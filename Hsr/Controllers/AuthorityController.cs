#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using Hsr.Data.Interface;
using Hsr.Model.Models;
using Hsr.Model.Models.ViewModel;
using Hsr.Models;

#endregion

namespace Hsr.Controllers
{
    public class AuthorityController : HsrBaseController
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IRepository<Authority> _authority;
        private readonly IRepository<Sysrole> _sysRoleRepository;

        public AuthorityController(IRepository<Menu> menuRepository, IRepository<Authority> authority,
            IRepository<Sysrole> sysRoleRepository)
        {
            _menuRepository = menuRepository;
            _sysRoleRepository = sysRoleRepository;
            _authority = authority;
        }

        public ActionResult Index()
        {
            return View();
        }
        
        public async Task<ActionResult> GetMenuTree(int? id, int? pid)
        {
            List<decimal?> menuIds = null;
            List<MenuVm> listNode=new List<MenuVm>();
            var pMenu = _menuRepository.TableNoTracking;
            if (id > 0)
            {
                if (pid > 0)
                {
                    var ids = _authority.TableNoTracking.Where(d => d.RoleId == pid).Select(d => d.Menuid);
                    if (ids.Any())
                    {
                        pMenu = pMenu.Where(d => ids.Contains(d.Id));
                        listNode = pMenu.ToList().ToMenuVms();
                    }
                    else
                    {
                        listNode=new List<MenuVm>();
                    }
                }
                else
                {
                    listNode = pMenu.ToList().ToMenuVms();
                }
                menuIds = _authority.TableNoTracking.Where(d => d.RoleId == id).Select(d => d.Menuid).ToList();
            }

            
            var mathched = await MenuToJsNodes(listNode, menuIds);

            return Json(mathched, JsonRequestBehavior.AllowGet);
        }

        private async Task<TreeJsonNode> MenuToJsNodes(List<MenuVm> menus, List<decimal?> needCheck)
        {
            var matchMenus = Task.Factory.StartNew(() =>
            {
                if (!menus.Any())
                {
                    return new TreeJsonNode();
                }
                var topMenus =
                    _menuRepository.TableNoTracking.Where(d => d.Layer.Value == 1)
                        .OrderBy(d => d.OrderNum)
                        .ToList()
                        .ToMenuVms()
                        .Select(d => d.ToJsonNode())
                        .ToList();
                foreach (var topMenu in topMenus)
                {
                    AssemblyChildNodes(topMenu, menus, needCheck.Select(d => d.ToString()));
                    if (!topMenu.children.Any())
                    {
                        topMenus.Remove(topMenu);
                    }
                }

                var treeNode = new TreeJsonNode();
                treeNode.pid = "-1";
                treeNode.children = topMenus;
                treeNode.id = "0";
                treeNode.text = "全部菜单";
                treeNode.state = new StateType {opened = true,Checked = true};
                return treeNode;
            });
            return matchMenus.Result;
        }

        private void AssemblyChildNodes(TreeJsonNode menu, List<MenuVm> allMenus, IEnumerable<string> needCheck)
        {
            var enumerable = needCheck as string[] ?? needCheck.ToArray();
            if (enumerable.Contains(menu.id))
            {
                menu.state.Checked = true;
            }
            menu.state.opened = true;
            var childs = allMenus.Where(d => d.Pid.ToString() == menu.id).OrderBy(d => d.OrderNum).ToList();
            if (childs.Any())
            {
                menu.children = childs.Select(d => d.ToJsonNode()).ToList();

                foreach (var child in menu.children)
                {
                    
                    AssemblyChildNodes(child, allMenus, enumerable);
                }
            }
        }

        [ActionName("Create")]
        public ActionResult Save(decimal?[] menuids, decimal? roleId)
        {
            menuids = menuids ?? new decimal?[] {};

            //获取当前角色修改前的权限
            var rawIds = _authority.TableNoTracking.Where(d => d.RoleId == roleId).Select(d => d.Menuid).ToArray();

            var common = menuids.Intersect(rawIds);
            var enumerable = common as decimal?[] ?? common.ToArray();
            var needDelIds = rawIds.Except(enumerable);
            var needAddIds = menuids.Except(enumerable).ToList();
            needAddIds.Remove(0);
            //获取此角色的所有子角色
            var subRoleIds =
                _sysRoleRepository.TableNoTracking.Where(d => d.RolePid == roleId).Select(d => d.RoleId).ToList();
            subRoleIds.Add(roleId);
            using (var scope = new TransactionScope())
            {
                try
                {
                    var delIds = needDelIds as decimal?[] ?? needDelIds.ToArray();
                    if (delIds.Any())
                    {
                        string delCommand =
                            string.Format("delete from AUTHORITY t where t.ROLE_ID in ({1}) and t.menuid in ({0})",
                                string.Join(",", delIds), string.Join(",", subRoleIds));
                        _authority.ExecuteSqlCommand(delCommand);
                    }

                    foreach (var menuId in needAddIds)
                    {
                        _authority.Insert(new Authority {Menuid = menuId, RoleId = roleId});
                    }
                    scope.Complete();
                }
                catch (Exception e)
                {
                    return Json(e.Message);
                }
            }


            return Json("1",JsonRequestBehavior.AllowGet);
        }

     
    }
}
