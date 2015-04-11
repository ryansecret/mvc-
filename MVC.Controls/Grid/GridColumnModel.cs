using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Dynamic;

namespace MVC.Controls.Grid
{
    public enum GridCellType
    {
        DEFAULT, INT, FLOAT, DATE, NONE, CUSTOM
    }

    /* Rquired to support fluent column mapping */
    public class GridColumnModelList<T> where T : class
    {
        public GridColumnModelList()
        {
            Items = new List<GridColumnModel>();
        }

        public List<GridColumnModel> Items { get; private set; }

        public GridColumnModel Add(Expression<Func<T, object>> expression)
        {
            GridColumnModel<T> column = new GridColumnModel<T>(expression);
            this.Items.Add(column);
            return column;
        }

        public Dictionary<string, object> GenerateValueDictionary(T item)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (GridColumnModel column in Items)
            {
                result.Add(column.Name, column.GetValue(item));
            }

            return result;
        }

        public void ForEach(Action<GridColumnModel> action)
        {
            Items.ForEach(action);
        }
    }

    public class GridColumnModel<T> : GridColumnModel where T : class
    {
        private Func<T, object> _function { get; set; }

        public GridColumnModel(Expression<Func<T, object>> expression)
            : base(expression.MemberWithoutInstance())
        {
            UpdateFromDataAnnotations(expression);
        }

        public override object GetValue(object arg)
        {
            return _function((T)arg);
        }

        internal void UpdateFromDataAnnotations(Expression<Func<T, object>> expression)
        {
            _function = expression.Compile();
            if (AttributeHelper.IsMember(expression))
            {
                DisplayAttribute display = AttributeHelper.GetMemberAttribute<T, DisplayAttribute>(expression, attr => attr.Name != null);
                if (display != null)
                {
                    SetCaption(display.Name);
                }

                EditableAttribute editable = AttributeHelper.GetMemberAttribute<T, EditableAttribute>(expression);
                if (editable != null)
                {
                    SetEditable(editable.AllowEdit);
                }

                if (GridControl.IsPrimaryKeyFunc != null)
                {
                    IsImplicitPrimaryKey = GridControl.IsPrimaryKeyFunc(AttributeHelper.GetMember(expression));
                }
                else
                {
                    IsImplicitPrimaryKey = AttributeHelper.HasMemberAttribute<T, KeyAttribute>(expression);
                }
            }
        }
    }

    public class GridColumnModel
    {
        private static readonly Dictionary<GridCellType, string> _columnCellTypes = new Dictionary<GridCellType, string>() { { GridCellType.INT, "int" }, { GridCellType.DATE, "date" }, { GridCellType.FLOAT, "float" } };

        private string _name, _caption, _index, _width, _align, _customAttributes = "", _onSelect = null;
        private GridCellType _cellType = GridCellType.DEFAULT;
        private bool _editable = true, _sortable = true, _hidden = false, _isPrimaryKey = false, _asGroup = false;
        public string _formatter = null, _unformatter = null;
        private IColumnRenderer _colRenderer = new TextColumnRenderer();
        private Dictionary<string, string> _clientEvents = new Dictionary<string, string>();

        public GridColumnModel() { }

        /// <summary>
        /// </summary>
        /// <param name="name">The name of the column as well as the member</param>
        public GridColumnModel(string name)
        {
            _name = name;
            _caption = _name;
            _width = "50";
            _align = "center";
        }

        /// <summary>
        /// </summary>
        /// <param name="name">The member name to use as data-source</param>
        /// <param name="caption">The column caption text</param>
        public GridColumnModel(string name, string caption)
        {
            _name = name;
            _caption = caption;
            _width = "50";
            _align = "center";
        }

        /// <summary>
        /// </summary>
        /// <param name="name">The member name to use as data-source</param>
        /// <param name="caption">The column caption text</param>
        /// <param name="width">The column width, percentage is allowed by: \"100%\"</param>
        public GridColumnModel(string name, string caption, string width)
        {
            _name = name;
            _caption = caption;
            _width = width;
            _align = "center";
        }

        /// <summary>
        /// </summary>
        /// <param name="name">The member name to use as data-source</param>
        /// <param name="caption">The column caption text</param>
        /// <param name="width">The column width, percentage is allowed by: \"100%\"</param>
        /// <param name="align">The column alignment</param>
        public GridColumnModel(string name, string caption, string width, string align)
        {
            _name = name;
            _caption = caption;
            _width = width;
            _align = align;
        }

        /// <summary>
        /// The member name to use as data source
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }

        /// <summary>
        /// Allows the additonal of custom attributes not mapped by the GridColumnModel object
        /// </summary>
        public string CustomAttributes { get { return _customAttributes; } set { _customAttributes = value; } }

        /// <summary>
        /// Whether or not the grid should be grouped by this column
        /// </summary>
        public bool AsGroup { get { return _asGroup; } set { _asGroup = value; } }

        /// <summary>
        /// An onCellSelecteed js callback. the method should accept 3 arguments: rowId, cellContent, eventObject
        /// </summary>
        public string OnCellSelect { get { return _onSelect; } set { _onSelect = value; } }

        /// <summary>
        /// The type of the cell. required to support sorting
        /// If you dont want to use one of the predefined cell-types, you can specify CUSTOM,
        /// And add the custom attributes manually using 
        /// </summary>
        public GridCellType CellType { get { return _cellType; } set { _cellType = value; } }

        public bool IsPrimaryKey { get { return _isPrimaryKey; } set { _isPrimaryKey = value; } }

        public bool IsImplicitPrimaryKey { get; protected set; }

        /// <summary>
        /// Specifies that the grid can be sorted by this column
        /// </summary>
        public bool IsSortable { get { return _sortable; } set { _sortable = value; } }

        public bool IsDefaultSort { get; set; }

        public bool IsEditable { get { return _editable; } set { _editable = value; } }

        /// <summary>
        /// Specifies that the column is hidden
        /// </summary>
        public bool IsHidden { get { return _hidden; } set { _hidden = value; } }

        /// <summary>
        /// Choose how the column will render when updating
        /// The default is <see cref="TextColumnRenderer"/>
        /// Other available: <seealso cref="ComboColumnRenderer" />
        /// </summary>
        /// <param name="renderer"></param>
        /// <returns></returns>
        public GridColumnModel SetColumnRenderer(IColumnRenderer renderer)
        { _colRenderer = renderer; return this; }


        /// <summary>
        /// Set an onCellSelecteed js callback, the method should accept 3 arguments: rowId, cellContent, eventObject
        /// </summary>
        /// <param name="cellSelect"></param>
        /// <returns></returns>
        public GridColumnModel SetOnCellSelect(string cellSelect)
        { _onSelect = cellSelect; return this; }

        /// <summary>
        /// Allows the addition of extra custom attributes to the current column
        /// </summary>
        /// <param name="customAttributes">one or more custom attributes. e.g. arg1='value',arg2='value'</param>
        /// <returns></returns>
        public GridColumnModel SetCustomAttributes(string customAttributes) { _customAttributes = customAttributes; return this; }

        /// <summary>
        /// Makes the grid grouped by the values in the current column
        /// </summary>
        /// <returns></returns>
        public GridColumnModel SetAsGroup() { _asGroup = true; return this; }

        public GridColumnModel SetIsDefaultSort() { IsDefaultSort = true; return this; }

        /// <summary>
        /// Sets the data type of the current column. Required to support sorting
        /// If you dont want to use one of the predefined cell-types, you can specify CUSTOM,
        /// And add the custom attributes manually <see cref="SetCustomAttributes"/>
        /// </summary>
        /// <param name="cellType">The cell data type</param>
        /// <returns></returns>
        public GridColumnModel SetCellType(GridCellType cellType) { _cellType = cellType; return this; }

        /// <summary>
        /// Sets the current column as the key value of each row
        /// This culumn value will be returned to the rowselected event
        /// </summary>
        /// <returns></returns>
        public GridColumnModel SetAsPrimaryKey() { _isPrimaryKey = true; return this; }

        public GridColumnModel SetFormatter(string formatter) { _formatter = formatter; return this; }
        public GridColumnModel SetUnformatter(string unformatter) { _unformatter = unformatter; return this; }


        /// <summary>
        /// Allows to add a javascript event for the specified column
        /// </summary>
        /// <param name="eventName">Any HTML-element event</param>
        /// <param name="funcName">The js function name without brackets</param>
        /// <returns></returns>
        public GridColumnModel AddEvent(string eventName, string funcName)
        {
            _clientEvents.Add(eventName, funcName);
            return this;
        }

        /// <summary>
        /// The member name to use as data source
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GridColumnModel SetName(string name)
        { _name = name; return this; }

        /// <summary>
        /// The column caption text
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        public GridColumnModel SetCaption(string caption)
        { _caption = caption; return this; }

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

        public virtual object GetValue(object arg)
        {
            throw new InvalidOperationException(string.Format("Column {0} doesn't have an expression specified!", _name));
        }

        /// <summary>
        /// Converst the Column to a serialized HTML string
        /// </summary>
        /// <returns></returns>
        public string Render(string gridName)
        {
            _colRenderer.Column = this;
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

            if (_cellType != GridCellType.CUSTOM)
            {
                if ((_sortable) && (_cellType != GridCellType.DEFAULT))
                    sb.Append("\", sorttype:\"" + _columnCellTypes[_cellType] + "\" ");
                else
                    sb.Append("\", sortable: " + _sortable.ToString().ToLower());
            }
            else
                sb.Append("\"");

            if ((this.IsPrimaryKey) || (this.IsImplicitPrimaryKey))
                sb.Append(", key: true");

            if (!string.IsNullOrEmpty(_formatter))
            {
                sb.Append(", formatter: " + _formatter);
            }
            else
            {
                if (_cellType == GridCellType.DATE)
                {
                    sb.Append(", formatter: 'date'");
                }
            }

            if (!string.IsNullOrEmpty(_unformatter))
            {
                sb.Append(", unformat: " + _unformatter);
            }

            sb.Append(", editable: ");
            sb.Append(_editable.ToString().ToLower());
            sb.Append(", hidden: ");
            sb.Append(_hidden.ToString().ToLower());

            if (!string.IsNullOrEmpty(_customAttributes))
                sb.Append(", " + _customAttributes);

            sb.AppendFormat(", {0}", _colRenderer.Render(gridName));
            sb.Append("}");
            return sb.ToString();
        }

        internal string renderDataEvents()
        {
            if (_clientEvents.Count == 0) return "";

            StringBuilder sb = new StringBuilder();
            sb.Append("dataEvents: [");

            foreach (string eventName in _clientEvents.Keys)
                sb.Append("{ type: '" + eventName + "', fn:function(e){" + _clientEvents[eventName] + "(e);}},");

            string res = sb.ToString();
            return res.Substring(0, res.Length - 1) + "]";
        }
    }
}
