using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Hsr.Core.Infrastructure;
using Hsr.Core.Log;
using Nop.Web.Framework.Mvc.Routes;

namespace Hsr.Plugin.RyanTest
{
    public class RouteProvider:IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            var logger = EngineContext.Current.Resolve<Log4Manager>();
            logger.Info("调用RegisterRoutes");
            routes.MapRoute("ryan", "ryantest-{controller}/{action}/{id}",
                new { controller = "PlugTest", action = "Index", id = UrlParameter.Optional },
                new[] { "Hsr.Plugin.RyanTest.Controllers" });
        }

        public int Priority {
            get { return 0; }
        }
    }
}
