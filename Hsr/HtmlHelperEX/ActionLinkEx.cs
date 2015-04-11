using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;

namespace Hsr.HtmlHelperEX
{
    public static class ActionLinkEx
    {
        public static MvcHtmlString ActionLinks(this HtmlHelper htmlHelper, string linkText, string actionName, string controllName, object routeValues=null, object htmlAttributes=null)
        {

            var controller = htmlHelper.ViewContext.Controller as HsrBaseController;
            if (controller != null)
            { 
                var canAccess = controller.Menus.Any(d => d.Action == actionName && d.Controller == controllName);
                if (canAccess)
                {
                    return htmlHelper.ActionLink(linkText, actionName, controllName, routeValues, htmlAttributes);
                }
            }

            return MvcHtmlString.Empty;     
        } 
    }
}