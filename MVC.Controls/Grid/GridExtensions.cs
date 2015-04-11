using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;

namespace MVC.Controls.Grid
{
    public static class GridExtensions
    {

        /// <summary>
        /// Creats a new jqGrid control MVC Wrapper
        /// </summary>
        /// <returns></returns>
        public static MvcHtmlString Grid(this HtmlHelper html, GridControl grid)
        {
            return new MvcHtmlString(Grid(grid));
        }

        /// <summary>
        /// Creats a new jqGrid control
        /// </summary>
        /// <returns></returns>
        public static string Grid(GridControl grid)
        {
            StringBuilder sb = new StringBuilder();

            /* Create HTML Tags */
            sb.AppendLine("<table id=\"" + grid.Name + "\"></table>");

            if (grid.Pager != null)
            {
                grid.Pager.GridName = grid.Name;
                sb.AppendLine(grid.Pager.RenderElement());
            }

            sb.Append(grid.RequiredData());

            /* Call the jQuery Grid plugin */
            sb.Append("<script language=\"javascript\">");
            sb.AppendLine("$(document).ready(function() {");

            sb.AppendLine("_grid_init(\"" + grid.Name + "\");");

            //sb.AppendLine("jQuery.extend(jQuery.jgrid.defaults,{emptyrecords: \"אין רשומות\",loadtext : \"בטעינה...\",pgtext : \"דף {0} מתוך {1}\", recordtext: \"רשומות {0} - {1} מתוך {2}\"});");

            sb.Append(grid.Render());
            sb.AppendLine(";");

            if (grid.Pager != null)
                sb.AppendLine(grid.Pager.RenderScript());

            if (!string.IsNullOrEmpty(grid.OnGridRenderCompleteEvent))
            {
                string fn = grid.OnGridRenderCompleteEvent;
                if (fn.IndexOf("(") == -1) fn = fn + "();";
                sb.AppendLine(grid.OnGridRenderCompleteEvent);
            }

            sb.AppendLine("});");
            sb.AppendLine("</script>");
            return sb.ToString();
        }

        /// <summary>
        /// Renderes a save button for the grid
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="buttonName">The button's name</param>
        /// <param name="buttonText">The button's text</param>
        /// <param name="gridName">The grid's name</param>
        /// <param name="htmlAttributes">the input's html attributes</param>
        /// <param name="clientEvent">js callback</param>
        /// <param name="allRows">Whether or not to save all rows, or only the selected one</param>
        /// <param name="bulkUrl">The controller action to use to save multiple changes at once, setting bulkUrl will automaticall set allRows to true</param>
        /// <param name="parameterName">The name of the Controller's action parameter the list of changed rows will be bound to</param>
        /// <returns></returns>
        public static string GridSaveButton(this HtmlHelper htmlHelper, string buttonName = "btnSave", string buttonText = "save", string gridName = "grid", IDictionary<string, object> htmlAttributes = null, string clientEvent = null, bool allRows = false, string bulkUrl = null, string parameterName = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
            sb.AppendLine("$(\"#" + buttonName + "\").click(function() {");

            if ((allRows) || (!string.IsNullOrEmpty(bulkUrl)))
            {
                if (!string.IsNullOrEmpty(clientEvent))
                    sb.AppendFormat("if (!{0}() return;\r\n", clientEvent);

                if (string.IsNullOrEmpty(bulkUrl))
                    sb.AppendFormat("saveAllChangedRows($('#{0}'));", gridName);
                else
                {
                    if (string.IsNullOrEmpty(parameterName))
                        throw new ApplicationException("When using Html.GridSaveButton with a bulkUrl, parameterName must be set as well");

                    sb.AppendFormat("gridSaveRows('{0}', '{1}', '{2}', null);", gridName, bulkUrl, parameterName);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(clientEvent))
                    sb.AppendFormat("if (!{0}({1}) return;\r\n", clientEvent, GridSelectedRow(htmlHelper, gridName));

                sb.AppendFormat("$(\"#{1}\").jqGrid('saveRow', {0}, aftersavefunc=updateButtonState($('#{1}')));\r\n", GridSelectedRow(htmlHelper, gridName), gridName);
            }
            sb.AppendLine("});");
            // sb.AppendLine("this.disabled = 'true';});");
            sb.AppendLine("</script>");

            TagBuilder tagBuilder = new TagBuilder("input");
            if (htmlAttributes != null)
                tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", "button", true);
            tagBuilder.MergeAttribute("value", buttonText, true);
            tagBuilder.MergeAttribute("name", buttonName);
            tagBuilder.MergeAttribute("id", buttonName);
            tagBuilder.MergeAttribute("gridMethod", "save_" + gridName);
            tagBuilder.MergeAttribute("allRows", allRows.ToString());
            //tagBuilder.MergeAttribute("style", "background:transparent; border:0", true);

            return tagBuilder.ToString() + sb.ToString();
        }

        /// <summary>
        /// Renders an update button for the grid
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="buttonName">The button's id</param>
        /// <param name="buttonText">The button's text</param>
        /// <param name="gridName">The grid's name</param>
        /// <param name="clientEvent">js callback</param>
        /// <param name="htmlAttributes">Additional html attributes for the button</param>
        /// <param name="rowNum">The row id to update. defaults to the selected row</param>
        /// <returns></returns>
        public static string GridUpdateButton(this HtmlHelper htmlHelper, string buttonName = "btnUpdate", string buttonText = "Update", string gridName = "grid", string clientEvent = null, IDictionary<string, object> htmlAttributes = null, string rowNum = null)
        {
            if (string.IsNullOrEmpty(rowNum)) rowNum = GridSelectedRow(htmlHelper, gridName);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
            sb.AppendLine("$(\"#" + buttonName + "\").click(function() {");

            if (!string.IsNullOrEmpty(clientEvent))
                sb.AppendFormat("if (!{0}({1})) return;\r\n", clientEvent, rowNum);

            sb.AppendFormat("$(\"#" + gridName + "\").jqGrid('editRow', {0});\r\n", rowNum);
            //sb.AppendLine("this.disabled = 'true';});");
            sb.AppendLine(" updateButtonState($('#" + gridName + "'));");
            sb.AppendLine("});");

            sb.AppendLine("</script>");

            TagBuilder tagBuilder = new TagBuilder("input");
            if (htmlAttributes != null)
                tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", "button", true);
            tagBuilder.MergeAttribute("value", buttonText, true);
            tagBuilder.MergeAttribute("name", buttonName);
            tagBuilder.MergeAttribute("id", buttonName);
            tagBuilder.MergeAttribute("gridMethod", "update_" + gridName);
            //tagBuilder.MergeAttribute("style", "background:transparent; border:0", true);

            return tagBuilder.ToString() + sb.ToString();
        }

        /// <summary>
        /// Renders a cancel button
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="buttonName">The button's id</param>
        /// <param name="buttonText">The button's text</param>
        /// <param name="gridName">The grid name</param>
        /// <param name="htmlAttributes">Additional html attributes for the button</param>
        /// <param name="clientEvent">js callback</param>
        /// <param name="allRows">Whether or not to cancel the entire changed rows as a hole, or just the selected</param>
        /// <returns></returns>
        public static string GridCancelButton(this HtmlHelper htmlHelper, string buttonName = "btnCancel", string buttonText = "Cancel", string gridName = "grid", IDictionary<string, object> htmlAttributes = null, string clientEvent = null, bool allRows = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
            sb.AppendLine("$(\"#" + buttonName + "\").click(function() {");

            if (allRows)
            {
                if (!string.IsNullOrEmpty(clientEvent))
                    sb.AppendFormat("if (!{0}() return;\r\n", clientEvent);

                sb.AppendFormat("cancelAllChangedRows($('#{0}'));", gridName);
            }
            else
            {
                if (!string.IsNullOrEmpty(clientEvent))
                    sb.AppendFormat("if (!{0}({1}) return;\r\n", clientEvent, GridSelectedRow(htmlHelper, gridName));

                sb.AppendFormat("$(\"#{1}\").jqGrid('restoreRow', {0});\r\n", GridSelectedRow(htmlHelper, gridName), gridName);
                sb.AppendFormat(" updateButtonState($('#{0}'));", gridName);
            }

            sb.AppendLine("});");
            sb.AppendLine("</script>");

            TagBuilder tagBuilder = new TagBuilder("input");
            if (htmlAttributes != null)
                tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", "button", true);
            tagBuilder.MergeAttribute("value", buttonText, true);
            tagBuilder.MergeAttribute("name", buttonName);
            tagBuilder.MergeAttribute("id", buttonName);
            tagBuilder.MergeAttribute("gridMethod", "cancel_" + gridName);
            tagBuilder.MergeAttribute("allRows", allRows.ToString());
            //tagBuilder.MergeAttribute("style", "background:transparent; border:0", true);

            return tagBuilder.ToString() + sb.ToString();
        }

        /// <summary>
        /// Renders an add button
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="buttonName">The button's id</param>
        /// <param name="buttonText">The buttons text</param>
        /// <param name="gridName">The grid's name</param>
        /// <param name="clientEvent">js callback</param>
        /// <param name="htmlAttributes">Additional html attributes for the button</param>
        /// <returns></returns>
        public static string GridAddButton(this HtmlHelper htmlHelper, string buttonName = "btnAdd", string buttonText = "Add", string gridName = "grid", string clientEvent = null, IDictionary<string, object> htmlAttributes = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
            sb.AppendLine("$(\"#" + buttonName + "\").click(function() {");

            if (!string.IsNullOrEmpty(clientEvent))
                sb.AppendFormat("if (!{0}()) return;\r\n", clientEvent);

            //sb.AppendLine("jQuery('#" + gridName + "').jqGrid('editGridRow','new',{height:280,reloadAfterSubmit:false});");
            sb.AppendFormat("gridAddRow('" + gridName + "');");
            //sb.AppendLine("this.disabled = 'true';});");
            sb.AppendLine(" updateButtonState($('#" + gridName + "'));");
            sb.AppendLine("});");

            sb.AppendLine("</script>");

            TagBuilder tagBuilder = new TagBuilder("input");
            if (htmlAttributes != null)
                tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", "button", true);
            tagBuilder.MergeAttribute("value", buttonText, true);
            tagBuilder.MergeAttribute("name", buttonName);
            tagBuilder.MergeAttribute("id", buttonName);
            tagBuilder.MergeAttribute("gridMethod", "add_" + gridName);

            return tagBuilder.ToString() + sb.ToString();
        }

        /// <summary>
        /// Renders a delete button
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="buttonName">The button's id</param>
        /// <param name="buttonText">The buttons text</param>
        /// <param name="gridName">The grid's name</param>
        /// <param name="actionUrl">The controller action to invoke. if null will use the DeleteUrl as defined in the grid's pager</param>
        /// <param name="clientEvent">js callback</param>
        /// <param name="htmlAttributes">Additional html attributes for the button</param>
        /// <returns></returns>
        public static string GridDeleteButton(this HtmlHelper htmlHelper, string buttonName = "btnDelete", string buttonText = "Delete", string gridName = "grid", string actionUrl = null, IDictionary<string, object> htmlAttributes = null, string clientEvent = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
            sb.AppendLine("$(\"#" + buttonName + "\").click(function() {");

            string update = string.Format(" updateButtonState($('#{0}'));", gridName);

            if (!string.IsNullOrEmpty(clientEvent))
                sb.AppendFormat("if (!{0}({1})) {{2} return;}\r\n", clientEvent, GridSelectedRow(htmlHelper, gridName), update);

            if (string.IsNullOrEmpty(actionUrl))
                sb.AppendFormat("$(\"#{1}\").jqGrid('delGridRow', {0});\r\n", GridSelectedRow(htmlHelper, gridName), gridName);
            else
            {
                sb.AppendFormat("$(\"#{1}\").jqGrid('delGridRow', {0}", GridSelectedRow(htmlHelper, gridName), gridName);
                sb.Append(", {url:'" + actionUrl + "'});\r\n");
            }

            sb.AppendFormat(update);

            sb.AppendLine("});");
            sb.AppendLine("</script>");

            TagBuilder tagBuilder = new TagBuilder("input");
            if (htmlAttributes != null)
                tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", "button", true);
            tagBuilder.MergeAttribute("value", buttonText, true);
            tagBuilder.MergeAttribute("name", buttonName);
            tagBuilder.MergeAttribute("id", buttonName);
            tagBuilder.MergeAttribute("gridMethod", "delete_" + gridName);
            //tagBuilder.MergeAttribute("style", "background:transparent; border:0", true);

            return tagBuilder.ToString() + sb.ToString();
        }

        /// <summary>
        /// Returns a javascript code that returns the selected grid row
        /// </summary>
        /// <param name="html"></param>
        /// <param name="gridName"></param>
        /// <returns></returns>
        public static string GridSelectedRow(this HtmlHelper html, string gridName = "grid")
        {
            return string.Format("$(\"#{0}\").jqGrid('getGridParam', 'selrow')", gridName);
        }

        /// <summary>
        /// Get the row at rowId model
        /// </summary>
        /// <param name="html"></param>
        /// <param name="gridName">The grid name</param>
        /// <param name="rowId">The row id, default to selected row</param>
        /// <returns></returns>
        public static string GridRowData(this HtmlHelper html, string gridName = "grid", string rowId = null)
        {
            if (string.IsNullOrEmpty(rowId)) rowId = GridSelectedRow(html, gridName);

            return "$(\"#" + gridName + "\").getRowData(" + rowId + ");";
        }
    }
}