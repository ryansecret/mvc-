#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

#endregion

namespace Hsr.Core.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizeFilterAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly bool _dontValidate;


        public AuthorizeFilterAttribute()
            : this(false)
        {
        }

        public AuthorizeFilterAttribute(bool dontValidate)
        {
            _dontValidate = dontValidate;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (_dontValidate)
                return;

            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
                throw new InvalidOperationException(
                    "You cannot use [Authorize] attribute when a child action cache is active");

            if (IsAdminPageRequested(filterContext) && HasAccess(filterContext))
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }

        private void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }

        private IEnumerable<AuthorizeFilterAttribute> GetAdminAuthorizeAttributes(ActionDescriptor descriptor)
        {
            return descriptor.GetCustomAttributes(typeof (AuthorizeFilterAttribute), true)
                .Concat(descriptor.ControllerDescriptor.GetCustomAttributes(typeof (AuthorizeFilterAttribute), true))
                .OfType<AuthorizeFilterAttribute>();
        }

        private bool IsAdminPageRequested(AuthorizationContext filterContext)
        {
            var adminAttributes = GetAdminAuthorizeAttributes(filterContext.ActionDescriptor);
            if (adminAttributes != null && adminAttributes.Any())
                return true;
            return false;
        }

        public virtual bool HasAccess(AuthorizationContext filterContext)
        {
            return filterContext.ActionDescriptor.GetCustomAttributes(typeof (AllowAnonymousAttribute), true)
                .OfType<AllowAnonymousAttribute>().Any();
        }
    }
}