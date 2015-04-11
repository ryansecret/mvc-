using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hsr.Core;

namespace Hsr.Controllers
{
    public class ErrorController:BaseController
    {
        public ActionResult HttpError()
        {
            return View("Error");
        }
        public ActionResult NotFound()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult NoPermision()
        {
            return View("NoPermision");
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}