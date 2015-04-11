using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Hsr.Model.Models;

namespace Hsr.Common
{
    public static class TreeEx
    {
        public static MvcHtmlString TreeFor(this HtmlHelper html, TreeNode nodes, string prefix = "")
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            TagBuilder uiTagBuilder = new TagBuilder("ul");
            TagBuilder liTagBuilder = new TagBuilder("li");
            liTagBuilder.MergeAttribute("id", nodes.NodeID.ToString());
            liTagBuilder.MergeAttribute("name", prefix+"_"+nodes.NodeID.ToString());
            liTagBuilder.MergeAttribute("data-jstree", "{ \"opened\" : true }");
            liTagBuilder.MergeAttribute("data-jstree", "{ \"selected\" : true }");
            liTagBuilder.SetInnerText(nodes.NodeName);
            StringBuilder str = new StringBuilder();
            if (nodes.IsLeaf == false)
            {
                foreach (var obj in nodes.Children)
                {
                    str.Append(HtmlNode(obj, prefix));
                }
            }
            TagBuilder uiTagBuilder2 = new TagBuilder("ul");
            uiTagBuilder2.InnerHtml = str.ToString();
            liTagBuilder.InnerHtml = nodes.NodeName + uiTagBuilder2.ToString();
            uiTagBuilder.InnerHtml = liTagBuilder.ToString();
            return new MvcHtmlString(uiTagBuilder.ToString(TagRenderMode.Normal));
        }
        private static TagBuilder HtmlNode(TreeNode node, string prefix)
        {
            TagBuilder liTagBuilder = new TagBuilder("li");
            liTagBuilder.MergeAttribute("id", node.NodeID.ToString());
            liTagBuilder.MergeAttribute("name", prefix + "_" + node.NodeID.ToString());
            liTagBuilder.SetInnerText(node.NodeName);
            if (node.IsSelected)
                {
                liTagBuilder.MergeAttribute("data-jstree", "{ \"opened\" : true }");
                liTagBuilder.MergeAttribute("data-jstree", "{ \"selected\" : true }");
                }
            if (node.IsLeaf == false)
            {
                TagBuilder ulTagBuilder = new TagBuilder("ul");
                ulTagBuilder.MergeAttribute("id", node.NodeID.ToString());

                ulTagBuilder.MergeAttribute("name", prefix + "_" + node.NodeID.ToString());

                StringBuilder str = new StringBuilder();
                foreach (var obj in node.Children)
                {
                    TagBuilder childrenliTagBuilder = new TagBuilder("li");
                    childrenliTagBuilder.SetInnerText(obj.NodeName);
                    if (obj.IsSelected == true)
                    {
                        childrenliTagBuilder.MergeAttribute("data-jstree", "{ \"opened\" : true }");
                        childrenliTagBuilder.MergeAttribute("data-jstree", "{ \"selected\" : true }");
                      
                    }
                    childrenliTagBuilder.MergeAttribute("id", obj.NodeID.ToString());
                    childrenliTagBuilder.MergeAttribute("name", prefix + "_" + obj.NodeID.ToString());
                    if (obj.IsLeaf == false)
                    {
                        childrenliTagBuilder = HtmlNode(obj, prefix);
                    }
                    str.Append(childrenliTagBuilder.ToString());
                }
                ulTagBuilder.InnerHtml = str.ToString();
                liTagBuilder.InnerHtml = node.NodeName + ulTagBuilder.ToString();
            }
         
            return liTagBuilder;
        }
    }
}