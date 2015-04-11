using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Hsr.Core;
using Hsr.Data.Interface;
using System.Data.Entity;
using Hsr.Service;
using Hsr.Model.Models;
using Hsr.Model.Models.ViewModel;
using WebGrease.Css.Extensions;
using System.Threading.Tasks;
using MVC.Controls;

namespace Hsr.Controllers
{
    public class SysroleController : HsrBaseController
    {
        private readonly IRepository<Sysrole> _sysrole;
 
        public SysroleController( IRepository<Sysrole> sysrole)
        {
            this._sysrole = sysrole;
        }
        //
        // GET: /Sysrole/
        [AllowAnonymous]
        public async Task<ActionResult> GetTree()
        {

            List<RoleVm> roles = await GetRoles();

            return PartialView("~/Views/Common/_Tree.cshtml", RoleToNodes(roles));
        }

        
        private TreeNode RoleToNodes(List<RoleVm> roles)
        {
            if (!roles.Any())
            {
                return new TreeNode();
            }
            var topRoles = roles.Where(d => d.RolePid  == 0).Select(d => d.ToNode()).ToList();
            foreach (var topRole in topRoles)
            {
                AssemblyChildNodes(topRole, roles);
            }
            var treeNode = new TreeNode();
            treeNode.ParentID = -1;
            treeNode.Children = topRoles;
            treeNode.NodeID = 0;
            treeNode.NodeName = "È«²¿½ÇÉ«";
            treeNode.IsSelected = true;
  
            return treeNode;
        }

        private void AssemblyChildNodes(TreeNode role, List<RoleVm> allroles)
        {
            var childs = allroles.Where(d => d.RolePid == role.NodeID).ToList();
            if (childs.Any())
            {
                role.Children = childs.Select(d => d.ToNode()).ToList();
                foreach (var child in role.Children)
                {
                    AssemblyChildNodes(child, allroles);
                }
            }
        }

        private async Task<List<RoleVm>> GetRoles(int? rolePid=null)
        {
            var data = Task.Factory.StartNew<List<RoleVm>>(() =>
            {
                var list = _sysrole.TableNoTracking;
                if (rolePid.HasValue)
                {
                   list= list.Where(d => d.RolePid == rolePid);
                }
                var roles = list.ToList().ToRoleVms();
                return roles;
            });
            return data.Result;
        }

        public async Task<ViewResult> Index(int? pid, RoleVms model)
        {
            var roles = await GetRoles(pid.HasValue?pid.Value:0);
            if(model.PageNumber<=0)
            {
                model.PageNumber = 1;
            }
            if(model.PageSize<=0)
            {
                model.PageSize = 2;
            }
            PagedList<RoleVm>pagedList=new PagedList<RoleVm>(roles,model.PageIndex,model.PageSize);
            model.LoadPagedList(pagedList);
            model.Data = pagedList;
            return View(model);
        }

        public async Task<ActionResult> GetList(int? pid,RoleVms model)     
        {
            
            if(model.PageNumber<=0)
            {
                model.PageNumber = 1;
            }
            if(model.PageSize<=0)
            {
                model.PageSize = 2;
            }
            var roles = await GetRoles(pid);
            PagedList<RoleVm>pagedlist=new PagedList<RoleVm>(roles,model.PageIndex,model.PageSize);
            model.LoadPagedList(pagedlist);
            model.Data = pagedlist;
            return PartialView("RoleList", model);
        }



        public ActionResult Create(int? pid)
        {
            return View(new RoleVm() { RolePid = pid });
        }

 
        [HttpPost]
        public ActionResult Create(RoleVm sysrole)
        {

            if (ModelState.IsValid)
            {
                sysrole.CreateTime = DateTime.Now;
                sysrole.CreateUserid = CurrentUser.Auid;
                sysrole.CreateBy = CurrentUser.UserName;
              
                _sysrole.Insert(sysrole.ToRole());
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

      

        public ActionResult Edit(decimal? id)
        {
            var vm = _sysrole.GetById(id).ToRoleVm();
            return View(vm);

        }
         
        public ActionResult Details(decimal? id)
        {
            var vm = _sysrole.GetById(id).ToRoleVm();
            return View(vm);

        }
        [HttpPost]
        public ActionResult Edit(RoleVm sysrole)
        {
            if (ModelState.IsValid)
            {
                _sysrole.Update(sysrole.ToRole());
                return RedirectToAction("Index", new { rolePid = sysrole.RolePid });
            }
            else
            {
                return View(sysrole);
            }
        }
       

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal? id)
        {
            try
            {
                using (TransactionScope transactionScope=new TransactionScope())
                {
                    string del = string.Format("delete from sysrole t where t.role_pid={0} or t.role_id={0}", id);
                    _sysrole.ExecuteSqlCommand(del);

                    del = string.Format("delete from authority t where t.role_id = ({0})",id);
                    _sysrole.ExecuteSqlCommand(del);
                    transactionScope.Complete();
                   
                }
                return Json("1");
            }
            catch (Exception)
            {
                return Json("0");
            }
        }

    }
}

