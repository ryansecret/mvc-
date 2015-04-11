using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC.Controls.Recaptcha
{
    public static class ValidateCodeHelper
    {
        private const string IdPrefix = "validateCode";
        private const int Length = 4;

        public static MvcHtmlString ValidateCode(this HtmlHelper helper)
        {
            return ValidateCode(helper, IdPrefix);
        }

        public static MvcHtmlString ValidateCode(this HtmlHelper helper, string id)
        {
            return ValidateCode(helper, id, Length);
        }

        public static MvcHtmlString ValidateCode(this HtmlHelper helper, string id, int length)
        {
            return ValidateCode(helper, id, length, null);
        }

        public static MvcHtmlString ValidateCode(this HtmlHelper helper, string id, object htmlAttributes)
        {
            return ValidateCode(helper, id, Length, htmlAttributes);
        }

        public static MvcHtmlString ValidateCode(this HtmlHelper helper, int length, object htmlAttributes)
        {
            return ValidateCode(helper, IdPrefix, length, htmlAttributes);
        }

        public static MvcHtmlString ValidateCode(this HtmlHelper helper, object htmlAttributes)
        {
            return ValidateCode(helper, 4, htmlAttributes);
        }

        public static MvcHtmlString ValidateCode(this HtmlHelper helper, int length)
        {
            return ValidateCode(helper, length, null);
        }

        public static MvcHtmlString ValidateCode(this HtmlHelper helper, string id, int length, object htmlAttributes)
        {
            string finalId = id + "_imgValidateCode";
            var tagBuild = new TagBuilder("img");
            tagBuild.GenerateId(finalId);
            var defaultController = ((Route)RouteTable.Routes["Default"]).Defaults["controller"] + "/";
            var controller = HttpContext.Current.Request.Url.Segments.Length == 1
                                 ? defaultController
                                 : HttpContext.Current.Request.Url.Segments[1];
            tagBuild.MergeAttribute("src", string.Format("/{0}GetValidateCode?length={1}", controller, length));
            tagBuild.MergeAttribute("alt", "");
            tagBuild.MergeAttribute("style", "cursor:pointer;");
            tagBuild.MergeAttributes(AnonymousObjectToHtmlAttributes(htmlAttributes));
            var sb = new StringBuilder();
            sb.Append("<script language=\"javascript\" type=\"text/javascript\">");
            sb.Append("$(document).ready(function () {");
            sb.AppendFormat("$(\"#{0}\").bind(\"click\", function () {{", finalId);
            //sb.Append("$(this).attr(\"style\", \"cursor:pointer;\");");
            sb.Append("var url = $(this).attr(\"src\");");
            sb.Append("url += \"&\" + Math.random();");
            sb.Append("$(this).attr(\"src\", url);");
            sb.Append("});");
            sb.Append("});");
            sb.Append("</script>");
            return MvcHtmlString.Create(tagBuild + sb.ToString());
        }

        public static RouteValueDictionary AnonymousObjectToHtmlAttributes(object htmlAttributes)
        {
            var result = new RouteValueDictionary();

            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    result.Add(property.Name.Replace('_', '-'), property.GetValue(htmlAttributes));
                }
            }

            return result;
        }
    }
}
