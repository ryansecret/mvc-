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
        /// Creats a new jqGrid control
        /// </summary>
        /// <param name="html"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static string Grid(this HtmlHelper html, GridControl grid)
        {
            StringBuilder sb = new StringBuilder();

            /* Create HTML Tags */
            sb.AppendLine("<table id=\"" + grid.GetGridName + "\"></table>");
            sb.AppendLine(createPager(grid.GetGridName));

            /* Call the jQuery Grid plugin */
            sb.Append("<script language=\"javascript\">");
            sb.AppendLine("$(document).ready(function() {");

            //sb.AppendLine("jQuery.extend(jQuery.jgrid.defaults,{emptyrecords: \"אין רשומות\",loadtext : \"בטעינה...\",pgtext : \"דף {0} מתוך {1}\", recordtext: \"רשומות {0} - {1} מתוך {2}\"});");

            sb.Append(grid.Render());

            sb.AppendLine("; $(\"#pg_" + grid.GetGridName + "Pager\").remove();});");

            sb.AppendLine("</script>");
            return sb.ToString();
        }

        private static string createPager(string gridName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div id=\"" + gridName + "Pager\">");

            sb.AppendLine("<table class=\"gridPagerTable\" align=\"center\">");
            sb.AppendLine("<tr style=\"height:17px;\">");
            sb.AppendLine("<td width=\"52\" id=\"gridPageStart\" onclick=\"grid_ChangePage('" + gridName + "',-2);\" class=\"gridPagerEndButtons\">&lt; Prev</td>");

            sb.AppendLine("<td width=\"18\" id=\"" + gridName + "Page1\" onclick=\"grid_ChangePage('" + gridName + "',1);\" align=\"center\" class=\"gridPagerSelectedButton\">1</td>");

            sb.AppendLine("<td width=\"55\" id=\"gridPageEnd\" onclick=\"grid_ChangePage('" + gridName + "',-1);\" class=\"gridPagerEndButtons\">Next &gt;</td>");

            sb.AppendLine("</tr></table>");

            sb.AppendLine("</div>");
            return sb.ToString();
        }

        #region Save

        public static string GridSaveButton(this HtmlHelper htmlHelper, string name, string buttonText, string gridName)
        {
            return GridSaveButton(htmlHelper, name, buttonText, gridName, null);
        }

        public static string GridSaveButton(this HtmlHelper htmlHelper, string name, string buttonText, string gridName, IDictionary<string, object> htmlAttributes)
        {
            return GridSaveButton(htmlHelper, name, buttonText, gridName, htmlAttributes, null);
        }

        public static string GridSaveButton(this HtmlHelper htmlHelper, string name, string buttonText, string gridName, IDictionary<string, object> htmlAttributes, string clientEvent)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
            sb.AppendLine("$(\"#" + name + "\").click(function() {");

            if (!string.IsNullOrEmpty(clientEvent))
                sb.AppendFormat("if (!{0}({1}) return;\r\n", clientEvent, GridSelectedRow(htmlHelper, gridName));

            sb.AppendFormat("$(\"#" + gridName + "\").Grid('saveRow', {0});\r\n", GridSelectedRow(htmlHelper, gridName));
            sb.AppendLine("this.disabled = 'true';});");
            sb.AppendLine("</script>");

            TagBuilder tagBuilder = new TagBuilder("input");
            if (htmlAttributes != null)
                tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", "button", true);
            tagBuilder.MergeAttribute("value", buttonText, true);
            tagBuilder.MergeAttribute("name", name);
            tagBuilder.MergeAttribute("id", name);
            //tagBuilder.MergeAttribute("style", "background:transparent; border:0", true);

            return tagBuilder.ToString() + sb.ToString();
        }

        #endregion


        #region Update


        public static string GridUpdateButton(this HtmlHelper htmlHelper, string name, string buttonText)
        {
            return GridUpdateButton(htmlHelper, name, buttonText, "grid");
        }

        public static string GridUpdateButton(this HtmlHelper htmlHelper, string name, string buttonText, string gridName)
        {
            return GridUpdateButton(htmlHelper, name, buttonText, gridName, "");
        }

        public static string GridUpdateButton(this HtmlHelper htmlHelper, string name, string buttonText, string gridName, string clientEvent)
        {
            return GridUpdateButton(htmlHelper, name, buttonText, gridName, clientEvent, null);
        }

        public static string GridUpdateButton(this HtmlHelper htmlHelper, string name, string buttonText, string gridName, string clientEvent, IDictionary<string, object> htmlAttributes)
        {
            return GridUpdateButton(htmlHelper, name, buttonText, gridName, clientEvent, htmlAttributes, GridSelectedRow(htmlHelper, gridName));
        }

        public static string GridUpdateButton(this HtmlHelper htmlHelper, string name, string buttonText, string gridName, string clientEvent, IDictionary<string, object> htmlAttributes, string rowNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
            sb.AppendLine("$(\"#" + name + "\").click(function() {");

            if (!string.IsNullOrEmpty(clientEvent))
                sb.AppendFormat("if (!{0}({1})) return;\r\n", clientEvent, rowNum);

            sb.AppendFormat("$(\"#" + gridName + "\").Grid('editRow', {0});\r\n", rowNum);
            sb.AppendLine("this.disabled = 'true';});");
            sb.AppendLine("</script>");

            TagBuilder tagBuilder = new TagBuilder("input");
            if (htmlAttributes != null)
                tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", "button", true);
            tagBuilder.MergeAttribute("value", buttonText, true);
            tagBuilder.MergeAttribute("name", name);
            tagBuilder.MergeAttribute("id", name);
            //tagBuilder.MergeAttribute("style", "background:transparent; border:0", true);

            return tagBuilder.ToString() + sb.ToString();
        }

        #endregion


        #region Cancel

        public static string GridCancelButton(this HtmlHelper htmlHelper, string name, string buttonText, string gridName)
        {
            return GridCancelButton(htmlHelper, name, buttonText, gridName, null);
        }

        public static string GridCancelButton(this HtmlHelper htmlHelper, string name, string buttonText, string gridName, IDictionary<string, object> htmlAttributes)
        {
            return GridCancelButton(htmlHelper, name, buttonText, gridName, htmlAttributes, null);
        }

        public static string GridCancelButton(this HtmlHelper htmlHelper, string name, string buttonText, string gridName, IDictionary<string, object> htmlAttributes, string clientEvent)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
            sb.AppendLine("$(\"#" + name + "\").click(function() {");

            if (!string.IsNullOrEmpty(clientEvent))
                sb.AppendFormat("if (!{0}({1}) return;\r\n", clientEvent, GridSelectedRow(htmlHelper, gridName));

            sb.AppendFormat("$(\"#" + gridName + "\").Grid('restoreRow', {0});\r\n", GridSelectedRow(htmlHelper, gridName));
            sb.AppendLine("this.disabled = 'true';});");
            sb.AppendLine("</script>");

            TagBuilder tagBuilder = new TagBuilder("input");
            if (htmlAttributes != null)
                tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", "button", true);
            tagBuilder.MergeAttribute("value", buttonText, true);
            tagBuilder.MergeAttribute("name", name);
            tagBuilder.MergeAttribute("id", name);
            //tagBuilder.MergeAttribute("style", "background:transparent; border:0", true);

            return tagBuilder.ToString() + sb.ToString();
        }

        #endregion

        /// <summary>
        /// Returns a javascript code that returns the selected grid row
        /// </summary>
        /// <param name="html"></param>
        /// <param name="gridName"></param>
        /// <returns></returns>
        public static string GridSelectedRow(this HtmlHelper html, string gridName)
        {
            return "$(\"#grid\").Grid('getGridParam', 'selrow')";
        }

        /// <summary>
        /// Returns a javascript code that returns the selected grid row
        /// </summary>
        /// <param name="html"></param>
        /// <param name="gridName"></param>
        /// <returns></returns>
        public static string GridSelectedRow(this HtmlHelper html)
        {
            return GridSelectedRow(html, "grid");
        }

        public static string GridRowData(this HtmlHelper html)
        {
            return GridRowData(html, "grid");
        }

        public static string GridRowData(this HtmlHelper html, string gridName)
        {
            return GridRowData(html, gridName, GridSelectedRow(html, gridName));
        }

        public static string GridRowData(this HtmlHelper html, string gridName, string rowId)
        {
            return "$(\"#" + gridName + "\").getRowData(" + rowId + ");";
        }
    }

    public class GridControl
    {
        private string _listUrl, _name, _pager, _editUrl, _caption;
        private string _listParams = null;
        private string _attributes = null;
        private string _onSelectedRow = null;
        private string _height = null, _width = null;

        private int _pageSize;
        private bool _isAutoSize, _isRowNumber, _isRTL = false;
        private GridControl _subGrid = null;

        private List<GridColumnModel> _columns = new List<GridColumnModel>();

        public GridControl()
        {
            this.isSubGrid = false;
        }

        private bool isSubGrid { get; set; }        
        public string GetGridName { get { return _name; } }
        public string GetPager { get { return _pager; } }

        public GridControl SetWidth(string val) { _width = val; return this; }
        public GridControl SetHeight(string val) { _height = val; return this; }

        /// <summary>
        /// Sets a javascript function name
        /// That will raise when a row is selected
        /// </summary>
        public GridControl SetOnSelectedRowEvent(string val) { _onSelectedRow = val; return this; }

        /// <summary>
        /// Sets the title of the grid
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        public GridControl SetCaption(string caption) { _caption = caption; return this; }

        /// <summary>
        /// The name of the div that will contain the grid
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GridControl SetName(string name) { _name = name; return this; }

        /// <summary>
        /// The name of the div that will contain the pager
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public GridControl SetPager(string pager) { _pager = pager; return this; }

        /// <summary>
        /// Sets the property Id to use to fetch the sub-grid's data
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public GridControl SetListQueryParams(string url) { _listParams = url; return this; }

        /// <summary>
        /// The url to the command that will return the list data of the grid
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public GridControl SetListUrl(string url) { _listUrl = url; return this; }

        /// <summary>
        /// The url to the command that will update the edited row
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public GridControl SetEditUrl(string url) { _editUrl = url; return this; }

        /// <summary>
        /// The page size
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public GridControl SetPageSize(int pageSize) { _pageSize = pageSize; return this; }

        /// <summary>
        /// Set wether the grid columns will autosize themself
        /// </summary>
        /// <param name="autoSize"></param>
        /// <returns></returns>
        public GridControl SetIsAutoSize(bool autoSize) { _isAutoSize = autoSize; return this; }

        /// <summary>
        /// Set wether to show row numbers
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <returns></returns>
        public GridControl SetIsRowNumber(bool rowNumber) { _isRowNumber = rowNumber; return this; }

        /// <summary>
        /// Add a column mapping
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public GridControl AddColumn(GridColumnModel column) { _columns.Add(column); return this; }

        /// <summary>
        /// Creates a sub grid
        /// </summary>
        /// <param name="subGrid"></param>
        /// <returns></returns>
        public GridControl CreateSubGrid(GridControl subGrid) { _subGrid = subGrid; return this; }


        /// <summary>
        /// Renderes the grid as RTL
        /// </summary>
        /// <returns></returns>
        public GridControl IsRTL() { _isRTL = true; return this; }

        /// <summary>
        /// Add additional custom parameters to the Grid
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public GridControl SetAdditionalAttributes(string attributes) { _attributes = attributes; return this; }

        public string Render()
        {
            StringBuilder sb = new StringBuilder();

            if (!this.isSubGrid)
            {
                sb.AppendLine("$(\"#" + _name + "\").jqGrid({");
                sb.AppendFormat("url: \"{0}\",\r\n", _listUrl);
            }
            else
            {
                sb.AppendLine("jQuery(\"#\" + subgrid_table_id).jqGrid({");
                sb.AppendFormat("url: \"{0},\r\n", _listUrl);
            }
            

            if (!string.IsNullOrEmpty(_editUrl)) sb.AppendFormat("editurl: \"{0}\",\r\n", _editUrl);

            sb.AppendFormat("mtype: \"get\",\r\n");
            sb.AppendFormat("datatype: \"json\",\r\n");
            sb.AppendFormat("colNames: [{0}],\r\n", renderColumnNames());
            sb.AppendFormat("colModel: [{0}],\r\n", renderColumnsModel());
            sb.AppendFormat("rowNum: {0},\r\n", _pageSize);

            if (!this.isSubGrid)
                sb.AppendLine("gridComplete: function f(){grid_initPager('" + _name + "');},");

            if (_isRTL)
                sb.AppendLine("direction: \"rtl\",");

            if (!string.IsNullOrEmpty(_height))
                sb.AppendFormat("height: {0},\r\n", _height);

            if (!string.IsNullOrEmpty(_width))
                sb.AppendFormat("width: {0},\r\n", _width);

            sb.AppendLine("rowList: -1,");

            if (!this.isSubGrid)
                sb.Append("pager: $(\"#" + _name + "Pager\"),");

            sb.AppendFormat("sortname: \"{0}\",\r\n", _columns[0].Name);

            sb.AppendFormat("autowidth: {0},\r\n", _isAutoSize.ToString().ToLower());

            if (!string.IsNullOrEmpty(_onSelectedRow))
            {
                sb.AppendLine("onSelectRow: function(id){" + _onSelectedRow + "(" +
                    "$(\"#" + _name + "\").getRowData(id));},");
            }
            else
            {
                sb.Append("onSelectRow: function(id){");
                sb.Append("if ($(\"#Id\").length == 0) {return;}");
                sb.Append("$(\"#Id\")[0].value = " + "$(\"#" + _name + "\").getRowData(id)." + getKeyColumnName() + ";},");
            }

            sb.AppendFormat("caption: \"{0}\"", _caption);

            if (!string.IsNullOrEmpty(_attributes)) sb.AppendLine(_attributes);

            if (_subGrid != null)
            {
                sb.AppendLine(",");
                sb.AppendLine("subGrid: true,");
                sb.AppendLine("subGridRowExpanded: function(subgrid_id, row_id) {");
                sb.AppendLine("var subgrid_table_id;");
                sb.AppendLine("subgrid_table_id = subgrid_id+\"_t\";");
                sb.AppendLine("$(\"#\"+subgrid_id).html(\"<table id='\"+subgrid_table_id+\"' class='scroll'></table>\");");

                _subGrid.isSubGrid = true;
                _subGrid._listUrl += "\" + $(\"#" + _name + "\").getRowData(row_id)." + getKeyColumnName();
                _subGrid._height = "\"100%\"";
                sb.AppendFormat("{0}{1}\r\n", _subGrid.Render(), "}");
            }

            sb.Append("})");

            return sb.ToString();
        }

        private string getKeyColumnName()
        {
            foreach (GridColumnModel col in _columns)
                if (col.IsPrimaryKey) return col.Name;

            throw new Exception("Grid Renderer Failed: Please choose a column as a primary key");
        }

        private string renderColumnNames()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _columns.Count; i++)
            {
                sb.Append("\"");
                sb.Append(_columns[i].GetColumnCaption());
                sb.Append("\"");
                if (i < _columns.Count - 1) sb.Append(",");
            }
            return sb.ToString();
        }

        private string renderColumnsModel()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _columns.Count; i++)
            {
                sb.Append(_columns[i].Render());
                if (i < _columns.Count - 1) sb.AppendLine(", ");
            }

            return sb.ToString();
        }
    }

    public class GridColumnModel
    {
        private string _name, _caption, _index, _width, _align;
        private bool _editable = true, _sortable = true, _hidden = false, _isPrimaryKey = false;

        public GridColumnModel() { }

        public GridColumnModel(string name)
        {
            _name = name;
            _caption = _name;
            _width = "50";
            _align = "center";
        }

        public GridColumnModel(string name, string caption)
        {
            _name = name;
            _caption = caption;
            _width = "50";
            _align = "center";
        }

        public GridColumnModel(string name, string caption, string width)
        {
            _name = name;
            _caption = caption;
            _width = width;
            _align = "center";
        }

        public GridColumnModel(string name, string caption, string width, string align)
        {
            _name = name;
            _caption = caption;
            _width = width;
            _align = align;
        }

        public string Name { get { return _name; } set { _name = value; } }
        public bool IsPrimaryKey { get { return _isPrimaryKey; } set { _isPrimaryKey = value; } }

        /// <summary>
        /// Sets the current column as the key value of each row
        /// This culumn value will be returned to the rowselected event
        /// </summary>
        /// <returns></returns>
        public GridColumnModel SetAsPrimaryKey() { _isPrimaryKey = true; return this; }

        /// <summary>
        /// The column title
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GridColumnModel SetName(string name)
        { _name = name; return this; }

        /// <summary>
        /// The name of the property to bind from the model
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public GridColumnModel SetIndex(string index)
        { _index = index; return this; }

        /// <summary>
        /// Set the width of the column
        /// (Overriding, if AutoSize was set)
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public GridColumnModel SetWidth(string width)
        { _width = width; return this; }

        /// <summary>
        /// Choose column aligment
        /// </summary>
        /// <param name="align"></param>
        /// <returns></returns>
        public GridColumnModel SetAlign(string align)
        { _align = align; return this; }

        /// <summary>
        /// Choose wether the column is editable or not
        /// </summary>
        /// <param name="editable"></param>
        /// <returns></returns>
        public GridColumnModel SetEditable(bool editable)
        { _editable = editable; return this; }

        /// <summary>
        /// Choose wether the column is sortable or not
        /// </summary>
        /// <param name="sortable"></param>
        /// <returns></returns>
        public GridColumnModel SetSortable(bool sortable)
        { _sortable = sortable; return this; }

        /// <summary>
        /// Choose wether the column is visible to the user or not
        /// </summary>
        /// <param name="hidden"></param>
        /// <returns></returns>
        public GridColumnModel SetHidden(bool hidden)
        { _hidden = hidden; return this; }

        public string GetColumnCaption() { return _caption; }

        /// <summary>
        /// Converst the Column to a serialized HTML string
        /// </summary>
        /// <returns></returns>
        public string Render()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");
            sb.Append("name: \"");
            sb.Append(_name);
            sb.Append("\", index: \"");
            if (string.IsNullOrEmpty(_index)) _index = _name;
            sb.Append(_index);
            sb.Append("\", width: ");
            sb.Append(_width);
            sb.Append(", align: \"");
            sb.Append(_align);
            sb.Append("\", sortable: ");
            sb.Append(_sortable.ToString().ToLower());
            sb.Append(", editable: ");
            sb.Append(_editable.ToString().ToLower());
            sb.Append(", hidden: ");
            sb.Append(_hidden.ToString().ToLower());
            sb.Append("}");
            return sb.ToString();
        }
    }
}
