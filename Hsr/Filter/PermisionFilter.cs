using System;
using System.Dynamic;
using System.Reflection;
using System.Web.Mvc;
using System.Linq;
namespace Hsr.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class PermisionFilterAttibute : ActionFilterAttribute
    {
        private const string IgnoreList = "Menu";
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller= filterContext.Controller as HsrBaseController;
            if (controller!=null)
            {
                var controllerName = filterContext.RouteData.Values["controller"].ToString();
                var actionName = filterContext.RouteData.Values["action"].ToString();
               

                Type type = controller.GetType();
                var notCheck =
                    type.GetMethods().Any(d => d.GetCustomAttribute<AllowAnonymousAttribute>() != null && d.Name == actionName);
                if (notCheck || actionName.StartsWith("Get") || IgnoreList.Contains(controllerName))
                {
                    return;
                }
                if (actionName == "Index")
                {
                    var menus = controller.Menus.Where(d => d.Controller == controllerName);
                    if (!menus.Any())
                    {
                        filterContext.Result = new RedirectResult(@"~\Error\NoPermision");
                    }
                }
                else
                {
                    var menus = controller.Menus.Where(d => d.Controller == controllerName && d.Action == actionName);
                    if (!menus.Any())
                    {
                        filterContext.Result = new RedirectResult(@"~\Error\NoPermision");
                    }
                }
            }
         
        }
 
    }
}
