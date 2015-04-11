using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace Hsr.Core.Filters
{
    public class AuthorizeFilter : IAuthorizationFilter
    {
        private bool _ready = false;
        public void OnAuthorization(AuthorizationContext filterContext)
        {
             
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
                throw new InvalidOperationException(
                    "You cannot use [Authorize] attribute when a child action cache is active");

            if (!HasAccess(filterContext))
            {
                HandleUnauthorizedRequest(filterContext);
            }
             
        }

        public virtual bool HasAccess(AuthorizationContext filterContext)
        {
           var result= filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true)
                .OfType<AllowAnonymousAttribute>().Any();
            if (result )
            {
                return true;
            }
            if (CanAccess(filterContext.RequestContext))
            {
                return true;
            }
            return false;
        }

        public bool CanAccess(RequestContext requestContext)
        {
            ;
            
            var user = requestContext.HttpContext.Session[CommonHelper.UserName];

            if (user!=null&&!string.IsNullOrWhiteSpace(user.ToString()))
            {
                return true;
            }
            else
            {
#if DEBUG
                if (requestContext.HttpContext.Request.Cookies.AllKeys.Contains(CommonHelper.UserName))
                {
                    requestContext.HttpContext.Session[CommonHelper.UserName] =
                        requestContext.HttpContext.Request.Cookies[CommonHelper.UserName].Value;
                    return true;
                }
#endif
            }
            return false;
        }

        private void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.Headers.AllKeys.Contains("X-Requested-With"))
            {
                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.HttpContext.Response.End();
            }
            else
            filterContext.Result = new HttpUnauthorizedResult();
           
        }
    }
}
