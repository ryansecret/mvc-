using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;
using MVC.Controls.Tab;
using System.IO;
using System.Web.Mvc.Html;
using MVC.Controls.Accordion;


namespace MVC.Controls
{
    public static class Manager
    {

       

        /// <summary>
        /// Creates a DatePicker for the current fie
        /// NOTE: the element must contain an id in the following format: id="THE-ELEMENT-ID"
        /// </summary>
        /// <param name="html"></param>
        /// <param name="datePicker"></param>
        /// <returns></returns>
        public static MvcHtmlString AsDatePicker(this MvcHtmlString html, DatePicker datePicker)
        {
            
            string elemId = getIdFromString(html);
            return new MvcHtmlString(html.ToHtmlString() + datePicker.Render(elemId));
        }

        public static MvcHtmlString Accordion(this HtmlHelper html, AccordionControl accCtrl)
        {
            return new MvcHtmlString(accCtrl.Render(html));
        }

        public static MvcHtmlString ProgressBar(this HtmlHelper html, ProgressBar bar)
        {
            return new MvcHtmlString(bar.Render());
        }
        public static MvcHtmlString Tab(this HtmlHelper html, TabControl tabCtrl)
        {
            return new MvcHtmlString(tabCtrl.Render(html));
        }

        /// <summary>
        /// Serializes the specified model object to a GET querystring parameter
        /// Note: The serialization is shallow, meaning only first level properties are serialized
        /// </summary>
        /// <param name="html"></param>
        /// <param name="model">The model object to serialize</param>
        /// <param name="parameterName">The parameter name of the Controller method</param>
        /// <returns></returns>
        public static string BuildQuery(this HtmlHelper html, object model, string parameterName)
        {

            PropertyInfo[] props = model.GetType().GetProperties();
            StringBuilder sb = new StringBuilder();
            sb.Append("?");
            bool isFirst = true;

            foreach (PropertyInfo prop in props)
            {
                object val = prop.GetValue(model, null);
                if (val != null)
                {
                    if (isFirst) isFirst = false;
                    else sb.Append("&");
                    sb.AppendFormat("{0}.{1}={2}", parameterName, prop.Name, val);
                }
            }

            return sb.ToString();
        }

        private static string getIdFromString(MvcHtmlString html)
        {
            string raw = html.ToHtmlString();
            if (raw.IndexOf("id=") == -1)
                throw new ArgumentException("MVC.Controls.DatePicker can only be used for MvcHtmlString with an html-id"
                    + Environment.NewLine + "[" + html.ToHtmlString() + "]");

            raw = raw.Substring(raw.IndexOf("id=") + 4);
            raw = raw.Substring(0, raw.IndexOf("\""));

            return raw;
        }

        internal static string RenderPartialToString(HtmlHelper htmlHelper, string viewName, object viewModel, ViewDataDictionary viewData)
        {
            StringWriter sw = new StringWriter();

            ViewContext newViewContext = new ViewContext(htmlHelper.ViewContext.Controller.ControllerContext,
                                                         new FakeView(),
                                                         htmlHelper.ViewContext.ViewData,
                                                         htmlHelper.ViewContext.TempData,
                                                         sw);

            var newHelper = new HtmlHelper(newViewContext, new ViewPage());
            newHelper.RenderPartial(viewName, viewModel, viewData);
             
            string res = sw.ToString();
            sw.Close();
            sw.Dispose();

            return res;
        }
    }

    internal class FakeView : IView
    {
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
