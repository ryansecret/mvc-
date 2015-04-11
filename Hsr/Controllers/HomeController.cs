using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hsr.Core;
using Hsr.Core.Cache;
using Hsr.Data.Interface;
using Hsr.Model.Models;
using Hsr.Model.Models.ViewModel;
using Hsr.Models;

namespace Hsr.Controllers
{
    
    public class HomeController : HsrBaseController
    {
        private readonly IRepository<Ryan> _repository;
        public readonly ICacheManager _CacheManager;
        private readonly IRepository<Menu> _menuRepository;
        public HomeController(IRepository<Ryan> repository, IRepository<Menu> menuRepository, ICacheManager cacheManager)
        {
            _repository = repository;
            _CacheManager = cacheManager;
            _menuRepository = menuRepository;
        }

        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            DateTime dateTime = new DateTime();
            var ss= dateTime.ToString("yyyy-MM-dd");
          
            //var ss= Request.RequestContext.HttpContext.Request.ApplicationPath;
            //var server= Request.ServerVariables;
            //var path = Request.UrlReferrer.PathAndQuery;
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                 
            }
            ViewBag.Message = "修改此模板以快速启动你的 ASP.NET MVC 应用程序。";
    
            return View();
        }

        [ChildActionOnly]
        //[OutputCache(Duration =60)]
        [AllowAnonymousAttribute]
        public ActionResult Menu()
        {
            
            var menus = Menus.Where(d=>d.Layer.HasValue).ToList().ToMenuVms();
            var topMenus = menus.Where(d => d.Layer == 1).OrderBy(d=>d.OrderNum).ToList();
            foreach (var menu in topMenus)
            {
                AssemblyChildNodes(menu, menus);
            }
             
            return PartialView("TopMenu", topMenus);
        }

        private void AssemblyChildNodes(MenuVm menu, List<MenuVm> allMenus)
        {
            var childs = allMenus.Where(d => d.Pid == menu.Id).OrderBy(d=>d.OrderNum).ToList();
            if (childs.Any())
            {
                menu.Children = childs;
                foreach (var child in childs)
                {
                    AssemblyChildNodes(child, allMenus);
                }
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "你的应用程序说明页。";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "你的联系方式页。";
          
            return View();
        }
    }
}
