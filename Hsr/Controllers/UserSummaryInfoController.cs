#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentValidation.Mvc;
using Hsr.Model.Models.ViewModel;
using Hsr.Models;
using Hsr.Service.Service;

#endregion

namespace Hsr.Controllers
{
    public class UserSummaryInfoController : HsrBaseController
    {
        //UserSummaryInfoVms 
        // GET: /UserSummaryInfo/
        private readonly IUserInfoService _userInfoService;

        public UserSummaryInfoController(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }


        public async Task<ActionResult> GetList(int? pid, UserSearParm searchparam, UserSummaryInfos model)
        {
            var list = await _userInfoService.GetUsersAsync(searchparam.Pid, searchparam, model);
            return PartialView("UserList", list);
        }

        public async Task<ActionResult> Index(UserSummaryInfos model)
        {
            var list = await _userInfoService.GetUsersAsync(0, null, model);
            return View(list);
        }


        
        public ActionResult Detail(string id)
        {
            return RedirectToAction("Edit", new {id=id});
        }

        //
        // GET: /ControllerFilterData/Create
        public ActionResult Create(int? selectedid, string selectname)
        {
            return View(new UserSummaryInfo
                {
                    Detail = new UserDetailInfo {CompId = selectedid, CompName = selectname},
                    Role = new List<UserRole>()
                });
        }

        //
        // POST: /ControllerFilterData/Create

        [HttpPost]
        public ActionResult Create([CustomizeValidator(RuleSet = "create")] UserSummaryInfo model)
        {
            if (ModelState.IsValid)
            {
                model.Detail.CreateBy = CurrentUser.UserName;
                model.Detail.CreateTime = DateTime.Now;
                model.Detail.CreateUserid = CurrentUser.Auid;
                _userInfoService.Insert(model);
                return Json(1);
            }
            return Json(0);
        }

        //
        // GET: /ControllerFilterData/Edit/5

        public ActionResult Edit(string id)
        {
            return View(_userInfoService.GetUserById(id));
        }

        //
        // POST: /ControllerFilterData/Edit/5

        [HttpPost]
        public ActionResult Edit([CustomizeValidator(RuleSet = "Edit")] UserSummaryInfo model)
        {
            if (ModelState.IsValid)
            {
                if (_userInfoService.Edit(model))
                {
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }


        public ActionResult CheckUserName(string userName)
        {
            return Json(_userInfoService.CheckUserName(userName), JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            _userInfoService.Delete(id);
            return Json("1",JsonRequestBehavior.AllowGet);
        }
    }
}