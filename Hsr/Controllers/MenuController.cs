#region

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using EntityFramework.Extensions;
using Hsr.Data.Interface;
using Hsr.Filter;
using Hsr.Model.Models;
using Hsr.Model.Models.ViewModel;
using Hsr.Service;
using Hsr.Service.Iservice;

#endregion

namespace Hsr.Controllers
{
    /// <summary>
    /// </summary>
    public class MenuController : HsrBaseController
    {
        /// <summary>
        ///     The _menu repository
        /// </summary>
        private readonly IRepository<Menu> _menuRepository;

        private readonly IDictionaryService _dictionaryService;
        /// <summary>
        ///     Initializes a new instance of the <see cref="MenuController" /> class.
        /// </summary>
        /// <param name="menuRepository">The menu repository.</param>
        public MenuController(IRepository<Menu> menuRepository,IDictionaryService dictionaryService)
        {
            _menuRepository = menuRepository;
            _dictionaryService = dictionaryService;
        }

        /// <summary>
        ///     Gets the tree.
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult GetTree(int? pid)
        {
            List<MenuVm> menus = null;

            var list = _menuRepository.Table.ToList();
            menus = list.ToMenuVms();


            if (pid.HasValue && menus != null)
            {
                var needSelecteds = menus.Where(d => d.Id == pid);

                if (needSelecteds.Any())
                {
                    MenuVm needSelected = needSelecteds.First();
                    var layer = needSelected.Layer;
                    while (layer > 0)
                    {
                        layer--;
                        needSelected.Seleted = true;
                        if (layer != 0)
                        {
                            needSelected = menus.First(d => d.Id == needSelected.Pid);
                        }
                    }
                }
            }
            return PartialView("~/Views/Common/_Tree.cshtml", MenuToNodes(menus));
        }


        public async Task<ActionResult> GetMenuTree(List<decimal?> menuIds)
        {
            var menus = await GetMenuByIds(menuIds);
            return PartialView("~/Views/Common/_JustTree.cshtml", MenuToNodes(menus));
        }

        private  Task<List<MenuVm>> GetMenuByIds(List<decimal?> menuIds)
        {
            var data = Task.Factory.StartNew<List<MenuVm>>(() =>
            {
                List<MenuVm> menus = null;
                var list = _menuRepository.Table;
                if (menuIds!=null&&menuIds.Any())
                {
                    list = list.Where(d => menuIds.Contains(d.Id.Value));
                }
                menus = list.ToList().ToMenuVms();
                menus.ForEach(d => d.Seleted = true);
                return menus;
            });
            return data;
        }
       

        /// <summary>
        ///     Gets the list.
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <returns></returns>
        public async Task<ActionResult> GetList(int pid)
        {
            var menus = await GetMenus(pid);

            return PartialView("MenuList", menus);
        }

        //
        // GET: /Menu/
        /// <summary>
        ///     Indexes the specified pid.
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <returns></returns>
     
        public ViewResult Index(int? pid)
        {
            int pId = pid ?? 0;
            var menus = GetMenuSync(pId);
            if (pid == 0)
            {
                TempData["menu"] = menus;
            }

            return View(menus);
        }

        /// <summary>
        ///     Gets the menu synchronize.
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <returns></returns>
        public List<MenuVm> GetMenuSync(int pid)
        {
            var list = _menuRepository.Table.Where(d => d.Pid == pid).OrderBy(d => d.OrderNum).ToList();
            var menus = list.ToMenuVms();
            return menus;
        }

        /// <summary>
        ///   Gets the menus.
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <returns></returns>
        private async Task<List<MenuVm>> GetMenus(int pid)
        {
            var data = Task.Factory.StartNew(() =>
            {
                var list = _menuRepository.Table.Where(d => d.Pid == pid&&d.Layer.HasValue).OrderBy(d => d.OrderNum).ToList();
                var menus = list.ToMenuVms();
                return menus;
            });
            return data.Result;
        }

       


        private TreeNode MenuToNodes(List<MenuVm> menus)
        {
            if (!menus.Any())
            {
                return new TreeNode();
            }
            var minLayer = menus.Min(d=>d.Layer);

            var topMenus = menus.Where(d => d.Layer != null && d.Layer.Value == minLayer).OrderBy(d => d.OrderNum).Select(d => d.ToNode()).ToList();
            foreach (var topMenu in topMenus)
            {
                AssemblyChildNodes(topMenu, menus);
            }
            var treeNode = new TreeNode();
            treeNode.ParentID = -1;
            treeNode.Children = topMenus;
            treeNode.NodeID = 0;
            treeNode.NodeName = "全部菜单";
            treeNode.IsSelected = true;
            return treeNode;
        }

        private void AssemblyChildNodes(TreeNode menu, List<MenuVm> allMenus)
        {
            var childs = allMenus.Where(d => d.Pid == menu.NodeID&&d.Layer.HasValue).OrderBy(d => d.OrderNum).ToList();
            if (childs.Any())
            {
                menu.Children = childs.Select(d => d.ToNode()).ToList();
                foreach (var child in menu.Children)
                {
                    AssemblyChildNodes(child, allMenus);
                }
            }
        }

        //
        // GET: /Menu/Details/5
        public ViewResult Details(decimal? id)
        {
            return View(_menuRepository.GetById(id).ToMenuVm());
        }

        //
        // GET: /Menu/Create
        
        public ActionResult Create(int pid = 0)
        {
            var item = new MenuVm {Pid = pid, Action = "Index", OrderNum = 1, Isenabled = true};
            if (pid!=0)
            {
                var actions=_dictionaryService.GetDictionaryInfos(Global.ForbiddenAction);

                ViewBag.ForbiddenActions = actions;
                item.ForbiddenAction = string.Join(",", actions.Select(d => d.Chname));
            }
            return View("Create", item);
        }

        //public ActionResult CreateTemplate(int pid = 0)
        //{
        //    return View("CreateTemplate", new MenuVm { Pid = pid, Action = "Delete", OrderNum = 1, Isenabled = true });
        //}

        //
        // POST: /Menu/Create

        [HttpPost]
        public ActionResult Create(MenuVm menu)
        {
            if (ModelState.IsValid)
            {
                var pMenu = _menuRepository.GetById(menu.Pid);
                if (pMenu != null)
                {
                    menu.PmenuName = pMenu.Menuname;
                    menu.Layer = pMenu.Layer + 1;
                }
                else
                {
                    menu.Layer = 1;
                }
                using (TransactionScope transactionScope=new TransactionScope())
                {
                    var insertItem = menu.ToMenu();
                    _menuRepository.Insert(insertItem);
                    List<Menu> items = new List<Menu>();
                    var actions = _dictionaryService.GetDictionaryInfos(Global.ForbiddenAction);
                    if (!string.IsNullOrWhiteSpace(insertItem.ForbiddenAction))
                    {
                        items.AddRange(insertItem.ForbiddenAction.Split(new[] { ',' }).Select(d => new Menu() { Pid = insertItem.Id, Action = actions.First(i => i.Chname == d).Enname, Controller = insertItem.Controller, Menuname = d }));
                    }
                    _menuRepository.BulkInsert(items);
                    transactionScope.Complete();
                }

                TempData.Remove("menu");
                return RedirectToAction("Index", new {pid = menu.Pid});
            }
            return View(menu);
        }

        //
        // GET: /Menu/Edit/5
        public ActionResult Edit(decimal? id)
        {
            var vm = _menuRepository.GetById(id).ToMenuVm();
            if (vm.Pid != 0)
            {
                var actions = _dictionaryService.GetDictionaryInfos(Global.ForbiddenAction);
                ViewBag.ForbiddenActions = actions;
            }
            vm.ForbiddenAction = string.Join(",", _menuRepository.TableNoTracking.Where(d=>d.Pid==id).Select(d => d.Menuname));
            return View(vm);
        }

        //
        // POST: /Menu/Edit/5
        [HttpPost]
        public ActionResult Edit(MenuVm menu)
        {
            if (ModelState.IsValid)
            {
                using (TransactionScope transactionScope=new TransactionScope())
                {
                    var editItem = menu.ToMenu();
                    _menuRepository.Update(menu.ToMenu());
                    var actions = _dictionaryService.GetDictionaryInfos(Global.ForbiddenAction);

                   var already=menu.Layer==1?new List<Menu>()  : _menuRepository.TableNoTracking.Where(d => d.Pid == menu.Id).ToList();

                    List<Menu> items = new List<Menu>();
                    if (!string.IsNullOrWhiteSpace(editItem.ForbiddenAction))
                    {
                        items.AddRange(editItem.ForbiddenAction.Split(new[] { ',' }).Select(d => new Menu() { Pid = editItem.Id, Action = actions.First(i => i.Chname == d).Enname, Controller = editItem.Controller, Menuname = d }));
                    }

                    var common= items.Intersect(already, new MenuComparer()).ToList();
                    var needAdd=  items.Except(common,new MenuComparer());
                    var needDel = already.Except(common,new MenuComparer()).ToList();
                    if (needDel.Any())
                    {
                        var ids = string.Join(",", needDel.Select(d => d.Id).ToList());
                          string sql = string.Format("Delete from menu t where  t.id in ({0})", ids);
                          _menuRepository.ExecuteSqlCommand(sql);
                        sql = string.Format("delete from authority t where t.menuid in ({0})", ids);
                        _menuRepository.ExecuteSqlCommand(sql);
                    }
                    _menuRepository.BulkInsert(needAdd.ToList());
                    
                    transactionScope.Complete();
                }
              
                TempData.Remove("menu");
                return RedirectToAction("Index", new {pid = menu.Pid});
            }
            return View(menu);
        }


        // POST: /Menu/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal? id, decimal? pId)
        {
             
            using (TransactionScope transactionScope = new TransactionScope())
            {
                string sql = string.Format("Delete from menu t where t.id={0} or t.pid={0}", id);
                _menuRepository.ExecuteSqlCommand(sql);
                _menuRepository.Table.Where(d => pId == id || d.Id == id).Delete();

                var subItems = _menuRepository.TableNoTracking.Where(d => d.Pid == id).ToList();
                if (subItems.Any())
                {

                    sql = string.Format("delete from authority t where t.menuid in ({0})", subItems.Select(d => d.Id));
                    _menuRepository.ExecuteSqlCommand(sql);
                }
                transactionScope.Complete();
            }
            TempData.Remove("menu");
            return RedirectToAction("GetTree", new {pid = pId});
        }


        internal class MenuComparer : IEqualityComparer<Menu>
        {
            public bool Equals(Menu x, Menu y)
            {
                return x.Controller == y.Controller&&x.Action==y.Action;
            }

            public int GetHashCode(Menu obj)
            {
                return this.ToString().GetHashCode();
            }
        }
    }
}