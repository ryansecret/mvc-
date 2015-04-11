using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MVC.Controls.Accordion
{
    public class AccordionControl
    {
        public string Name { get; set; }
        public bool CacheTabs { get; set; }
        public string LoadingText { get; set; }
        public string AdditionalAttributes { get; set; }
        public List<AccordionItem> AccordionItems { get; set; }


        public AccordionControl()
        {
            this.AccordionItems = new List<AccordionItem>();
            this.LoadingText = "Loading...";
            this.Name = "accordion";
            this.CacheTabs = false;
        }


        /// <summary>
        /// If set, will cache remote action accordion items, so that only the first time
        /// They will cal the server
        /// </summary>
        /// <returns></returns>
        public AccordionControl SetCacheTabs() { this.CacheTabs = true; return this; }

        /// <summary>
        /// Set the id of the tabs container
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AccordionControl SetName(string name) { this.Name = name; return this; }

        /// <summary>
        /// Set the text to be shown until a remote action accordion has finished loading
        /// </summary>
        /// <param name="txt">The loading text</param>
        /// <returns></returns>
        public AccordionControl SetLoadingText(string txt) { this.LoadingText = txt; return this; }

        /// <summary>
        /// Set additional attributes not already mapped
        /// </summary>
        /// <param name="additionalAttributes"></param>
        /// <returns></returns>
        public AccordionControl SetAdditionalAttributes(string additionalAttributes) { this.AdditionalAttributes = additionalAttributes; return this; }

        /// <summary>
        /// Add a new AccordionItem
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public AccordionControl AddAccordionItem(AccordionItem item) { this.AccordionItems.Add(item); return this; }



        public string Render(HtmlHelper htmlHelper)
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("_accordion_loadingText = \"" + this.LoadingText + "\";\r\n");
            sb.Append("$(\"#" + this.Name + "\").accordion({");
            sb.Append("active:-1, clearStyle: true, collapsible: true, changestart: _accordion_changeItem");
            
            /* Additional Parameters */
            if (!string.IsNullOrEmpty(this.AdditionalAttributes))
                sb.Append(", " + this.AdditionalAttributes);

            sb.AppendLine("});");

            if (getSelected() != -1)
                sb.AppendLine("$(\"#" + this.Name + "\").accordion(\"activate\", " + getSelected() + ");");

            sb.AppendLine("$(\"#" + this.Name + "\").attr(\"mvc_cacheItems\"," + this.CacheTabs.ToString().ToLower() + ");");

            string script = string.Format(MVCControlsScriptManager.SCRIPT_TEMPLATE, sb.ToString());

            sb.Clear();
            int index = 0;
            //StringBuilder partialViews = new StringBuilder();

            foreach (AccordionItem item in this.AccordionItems)
            {
                string tabContent = "";

                sb.AppendLine(item.Render(htmlHelper, index));

                switch (item.ContentType)
                {
                    case ContentType.Static:
                        tabContent = item.HTMLContent;
                        break;
                    case ContentType.PartialView:
                        tabContent = Manager.RenderPartialToString(htmlHelper, item.PartialViewName, item.PartialViewModel, item.PartialViewData);
                        break;
                    case ContentType.RemoteAction:
                        tabContent = this.LoadingText;
                        break;
                }

                sb.AppendLine("<div>" + tabContent + "</div>");

                index++;
            }
            string html = string.Format("<div id=\"" + this.Name + "\">\r\n{0}\r\n</div>", sb.ToString());

            return html + script;
        }

        private int getSelected()
        {
            for (int i = 0; i < this.AccordionItems.Count; i++)
                if (this.AccordionItems[i].Selected)
                    return i;

            return -1;
        }
    }
}
