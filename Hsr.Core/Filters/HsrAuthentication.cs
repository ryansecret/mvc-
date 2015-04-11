using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.UI.WebControls;

namespace Hsr.Core.Filters
{
    public class HsrAuthentication : IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
           var controller=  filterContext.RequestContext.RouteData.Values["controller"].ToString();
           if (controller.ToLower().Contains("menu"))
            {
               // filterContext.Result = new RedirectResult();
            }

        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            
        }
    }
}
