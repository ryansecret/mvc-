using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Controls.Grid
{
    public class CustomGridPagerControl : IGridPagerControl
    {
        private string _name = null;

        public string Name 
        {
            get
            {
                if (string.IsNullOrEmpty(_name)) _name = this.GridName + "Pager";

                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string GridName { get; set; }

        public string RenderElement()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div id=\"" + this.Name + "\">");

            sb.AppendLine("<table class=\"gridPagerTable\" align=\"center\">");
            sb.AppendLine("<tr style=\"height:17px;\">");
            sb.AppendLine("<td width=\"52\" id=\"gridPageStart\" onclick=\"grid_ChangePage('" + this.GridName + "',-2);\" class=\"gridPagerEndButtons\">&lt; Prev</td>");

            sb.AppendLine("<td width=\"18\" id=\"" + this.GridName + "Page1\" onclick=\"grid_ChangePage('" + this.GridName + "',1);\" align=\"center\" class=\"gridPagerSelectedButton\">1</td>");

            sb.AppendLine("<td width=\"55\" id=\"gridPageEnd\" onclick=\"grid_ChangePage('" + this.GridName + "',-1);\" class=\"gridPagerEndButtons\">Next &gt;</td>");

            sb.AppendLine("</tr></table>");

            sb.AppendLine("</div>");
            return sb.ToString();
        }

        public string RenderScript()
        {
            return ("$(\"#pg_" + this.Name + "\").remove();");
        }

        public string OnGridLoad()
        {
            return "grid_initPager('" + this.GridName + "'); ";
        }
    }
}
