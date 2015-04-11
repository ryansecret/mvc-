using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Hsr.Core.Infrastructure;
using Nop.Web.Framework.Mvc.Routes;

namespace Hsr
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
             
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
          
            var routePublisher = EngineContext.Current.Resolve<IRoutePublisher>();
            routePublisher.RegisterRoutes(routes);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },

                namespaces: new[] { "Hsr.Controllers" }
            );
            routes.MapRoute(name: "NoPermission", url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" });
            //routes.MapPageRoute("new", "Upload/UpLoadSingle.ashx", "~/Upload/UpLoadSingle.ashx");
        }
    }
}