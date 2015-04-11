using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Ajax; 

namespace Hsr.Core.HtmlEx
{
    public static class AjaxEx
    {
        /// <summary>
        /// Textboxes the specified ajax helper.
        ///  @Ajax.Textbox("search",
        ///new AjaxOptions {Url = @Url.Action("GetTime") ,UpdateTargetId = "divTime",InsertionMode = InsertionMode.Replace
         ///}, new { size = 50 })
        /// </summary>
        /// <param name="ajaxHelper">The ajax helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString Textbox(this AjaxHelper ajaxHelper, string name,
    AjaxOptions ajaxOptions, object htmlAttributes)
        {
            var tag = new TagBuilder("input");
            tag.MergeAttribute("name", name);
            tag.MergeAttribute("type", "text");

            tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            tag.MergeAttributes((ajaxOptions ?? new AjaxOptions()).ToUnobtrusiveHtmlAttributes());

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString AjaxActionWithImage(this AjaxHelper html, string imgSrc, string actionName, object routeValue = null, AjaxOptions ajaxOptions = null)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);

            string imgUrl = urlHelper.Content(imgSrc);
            TagBuilder imgTagBuilder = new TagBuilder("img");
            imgTagBuilder.MergeAttribute("src", imgUrl);
            string img = imgTagBuilder.ToString(TagRenderMode.SelfClosing);
            string url = urlHelper.Action(actionName, urlHelper.RequestContext.RouteData.Values["controller"].ToString(), routeValue);

            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = img
            };

            tagBuilder.MergeAttribute("href", url);
            tagBuilder.MergeAttributes((ajaxOptions ?? new AjaxOptions()).ToUnobtrusiveHtmlAttributes());
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}
