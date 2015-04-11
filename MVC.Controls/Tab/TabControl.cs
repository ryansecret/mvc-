using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.IO;
using System.Web.UI;
using System.Web;

namespace MVC.Controls.Tab
{
    public class TabControl
    {
        public string Name { get; set; }
        public string OnSelect { get; set; }
        public bool CacheTabs { get; set; }
        public string Spinner { get; set; }
        public string AdditionalAttributes { get; set; }
        public string ContainerCss { get; set; }
        public List<TabItem> TabItems{get;set;}

        public TabControl()
        {
            this.TabItems = new List<TabItem>();
            this.Name = "tabContainer";
            this.Spinner = "Loading...";
        }

        /// <summary>
        /// Set the id of the tabs container
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TabControl SetName(string name) { this.Name = name; return this; }

        /// <summary>
        /// Set a js callback method for the onSelect event, could be used for validation
        /// </summary>
        /// <param name="onSelect"></param>
        /// <returns></returns>
        public TabControl SetOnSelect(string onSelect) { this.OnSelect = onSelect; return this; }

        /// <summary>
        /// Whether or not to cache the remote tabs content
        /// </summary>
        /// <param name="cacheTabs"></param>
        /// <returns></returns>
        public TabControl SetCacheTabs(bool cacheTabs) { this.CacheTabs = cacheTabs; return this; }

        /// <summary>
        /// Set the string to be shown at the tab's title while loading
        /// </summary>
        /// <param name="spinner"></param>
        /// <returns></returns>
        public TabControl SetSpinner(string spinner) { this.Spinner = spinner; return this; }

        /// <summary>
        /// Additional Attributes
        /// </summary>
        /// <param name="additionalAttributes"></param>
        /// <returns></returns>
        public TabControl SetAdditionalAttributes(string additionalAttributes) { this.AdditionalAttributes = additionalAttributes; return this; }

        /// <summary>
        /// Allows specifying a custom css class for the tab container
        /// </summary>
        /// <param name="containerCss"></param>
        /// <returns></returns>
        public TabControl SetContainerCss(string containerCss) { this.ContainerCss = containerCss; return this; }
        
        /// <summary>
        /// Add a tab
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public TabControl AddTabItem(TabItem item) { this.TabItems.Add(item); return this; }


        public string Render(HtmlHelper htmlHelper)
        {
            StringBuilder sb = new StringBuilder();
            string css = "";
            if (!string.IsNullOrEmpty(this.ContainerCss)) css = "class=\"" + this.ContainerCss + "\"";

            /* Default tab */
            sb.Append("selected: " + getSelected());

            /* On Select js callback */
            if (!string.IsNullOrEmpty(this.OnSelect)) 
                sb.Append(", select: function(event, ui){return " + this.OnSelect + "(event, ui);}");
            
            /* Cache remote tabs? */
            if (this.CacheTabs)
                sb.Append(", cache: true");

            /* Disable Tabs */
            string disabledIndexes = getDisabledList();
            if (!string.IsNullOrEmpty(disabledIndexes))
                sb.Append(", disabled: [" + disabledIndexes + "]");
            
            /* Spinner */
            if (!string.IsNullOrEmpty(this.Spinner))
                sb.Append(", spinner: \"" + this.Spinner + "\"");

            /* Additional Parameters */
            if (!string.IsNullOrEmpty(this.AdditionalAttributes))
                sb.Append(", " + this.AdditionalAttributes);


            string script = string.Format(MVCControlsScriptManager.SCRIPT_TEMPLATE, "$(\"#" + this.Name + "\").tabs({" + sb.ToString() + "});");

            sb.Clear();
            int index = 0;
            StringBuilder partialViews = new StringBuilder();

            foreach (TabItem item in this.TabItems)
            {
                string tabContent = "";

                switch (item.ContentType)
                {
                    case ContentType.Static:
                        tabContent = item.HTMLContent;
                        item.Action = "#tabItem_" + index;
                        break;
                    case ContentType.PartialView:
                        item.Action = "#tabItem_" + index;
                        tabContent = Manager.RenderPartialToString(htmlHelper, item.PartialViewName, item.PartialViewModel, item.PartialViewData);
                         
                        break;
                    case ContentType.RemoteAction:
                        break;
                }

                if (item.ContentType != ContentType.RemoteAction)
                    partialViews.Append("<div id=\"tabItem_" + index + "\">" + tabContent + "</div>");

                sb.Append(item.Render(htmlHelper, index));

                index++;
            }
            string html = string.Format("<div id=\"{0}\" {1}><ul>{2}</ul>{3}</div>", this.Name, css, sb.ToString(), partialViews.ToString());

            return html + script;
        }


        private string getDisabledList()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.TabItems.Count; i++)
                if (!this.TabItems[i].Enabled)
                    sb.Append(", " + i);

            if (sb.Length != 0)
                return sb.ToString().Substring(2);
            return
                null;
        }

        private int getSelected()
        {
            for (int i = 0; i < this.TabItems.Count; i++)
                if (this.TabItems[i].Selected)
                    return i;

            return 0;
        }
    }

    
}
