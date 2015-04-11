using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hsr.Core;
using Hsr.Filter;
using Hsr.Model.Models;
using Hsr.Models;

namespace Hsr
{
    [PermisionFilterAttibute]
    public class HsrBaseController:BaseController
    {
        private List<Menu> _menus;
        private AreaInfo _userBelong;
        private bool _permissionCheck=true;
         
        public List<Menu> Menus
        {
            get { return Session["Menu"] as List<Menu>; }
            set { Session["Menu"] = value; }
        }

        public AreaInfo UserBelong
        {
            get { return Session["UserBelong"] as AreaInfo; }
            set { Session["UserBelong"] = value; }
        }

        public UserSummaryInfo CurrentUser
        {
            get
            {
                return Session["CurrentUser"] as UserSummaryInfo;
            }
            set
            {
                Session["CurrentUser"] = value;
            }
        }

        public bool PermissionCheck
        {
            get { return _permissionCheck; }
            set { _permissionCheck = value; }
        }

        //private UserSummaryInfo GetCurrentUser()
        //{

        //}
    }
}