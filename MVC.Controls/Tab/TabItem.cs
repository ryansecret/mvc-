using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MVC.Controls.Tab
{
    public class TabItem
    {

        public string Title { get; set; }
        public string Css { get; set; }
        public bool Selected { get; set; }
        public bool Enabled { get; set; }

        public ContentType ContentType { get; set; }

        /// <summary>
        /// If ContentType is set to RemoteAction, defines the remote action relative url
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// If ContentType is set to Static, defines the HTML content
        /// </summary>
        public string HTMLContent { get; set; }

        /* If ContentType is set to PartialView, defines the PartialView rendering details */
        public string PartialViewName { get; set; }
        public object PartialViewModel { get; set; }
        public ViewDataDictionary PartialViewData { get; set; }
        
        public TabItem()
        {
            this.Enabled = true;
            this.Selected = false;
            this.ContentType = Controls.ContentType.Static;
            this.PartialViewName = null;
        }
        
        /// <summary>
        /// Set the title of the tab
        /// </summary>
        /// <param name="title">The title</param>
        /// <returns></returns>
        public TabItem SetTitle(string title) { this.Title = title; return this; }

        /// <summary>
        /// Sets the css class of the tab title
        /// </summary>
        /// <param name="css">The css class name</param>
        /// <returns></returns>
        public TabItem SetCss(string css) { this.Css = css; return this; }

        /// <summary>
        /// Whether or not this tab is the default tab selected
        /// </summary>
        /// <returns></returns>
        public TabItem SetSelected() { this.Selected = true; return this; }

        /// <summary>
        /// Whether or not this tab is enabled
        /// </summary>
        /// <returns></returns>
        public TabItem SetDisabled() { this.Enabled = false; return this; }

        /// <summary>
        /// Set the content of the current Tb
        /// </summary>
        /// <param name="contentType">Determines how the content of the tab is to be rendered</param>
        /// <param name="name">If contentType is Static, determines the actual HTML, if contentType is PartialView, determines the view name, otherwise determines the action url</param>
        /// <param name="objectModel">Only when contentType is PartialView. The model to be passed to the view</param>
        /// <param name="viewData">Only when contentType is PartialView. The view data dictionary to be passed to the view</param>
        /// <returns></returns>
        public TabItem SetContent(ContentType contentType, string name, object objectModel = null, ViewDataDictionary viewData = null)
        {

            this.ContentType = contentType;

            switch (contentType)
            {
                case ContentType.Static:
                    this.HTMLContent = name;
                    break;
                case Controls.ContentType.PartialView:
                    this.PartialViewName = name;
                    this.PartialViewModel = objectModel;
                    this.PartialViewData = viewData;
                    break;
                case Controls.ContentType.RemoteAction:
                    this.Action = name;
                    break;
            }

            
            return this;
        }

        public virtual string Render(HtmlHelper htmlHelper, int index)
        {
            string css = (string.IsNullOrEmpty(this.Css)) ? "" : "clss=\"" + this.Css + "\"";
            return string.Format("<li><a href=\"{0}\"><span {1}>{2}</span></a></li>", this.Action, css, this.Title);
        }
    }
}
