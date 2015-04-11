using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Owin;

namespace Hsr.App_Start
{
    public class Startup  
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }

    }
}
