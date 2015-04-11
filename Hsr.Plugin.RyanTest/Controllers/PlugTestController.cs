using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Hsr.Core;
using Hsr.Core.Infrastructure;
using Hsr.Core.Log;

namespace Hsr.Plugin.RyanTest.Controllers
{
    public class PlugTestController:BaseController
    {
        public ActionResult Index()
        {
            var logger = EngineContext.Current.Resolve<Log4Manager>();
            logger.Info("调用PlugTest/index");
          // return View("~/Plugins/RyanTest/Views/PlugTest/Index.cshtml");
          return View();
        }
    }
}
