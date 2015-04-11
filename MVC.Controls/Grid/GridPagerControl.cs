using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Controls.Grid
{

    public interface IGridPagerControl
    {
        /// <summary>
        /// The Pager element name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The parent grid name
        /// </summary>
        string GridName { get; set; }

        /// <summary>
        /// a jscript code executed after the grid initialized
        /// </summary>
        /// <returns></returns>
        string RenderScript();

        /// <summary>
        /// a jscript code executed after the grid loaded
        /// </summary>
        /// <returns></returns>
        string OnGridLoad();

        /// <summary>
        /// The html code of the pager
        /// </summary>
        /// <returns></returns>
        string RenderElement();
    }

    public class GridPagerControl : IGridPagerControl
    {
        private string _name = null;
        private int? _addEditWidth = null;
        private int? _addEditHeight = null;

        public GridPagerControl()
        {
            Refresh = true;
            Search = true;
            MultipleSearch = true;

            SetHandleMvcResponse();
            ExtraAddAttributes = new List<string>();
            ExtraEditAttributes = new List<string>();
        }

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    if (!string.IsNullOrEmpty(this.GridName))
                        _name = this.GridName + "Pager";
                }

                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string GridName { get; set; }

        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Del { get; set; }
        public bool Search { get; set; }
        public bool MultipleSearch { get; set; }
        public bool View { get; set; }
        public bool Refresh { get; set; }
        public string AdditionalArguments { get; set; }
        public string DeleteUrl { get; set; }
        public string AfterSubmit { get; set; }
        public List<string> ExtraAddAttributes { get; set; }
        public List<string> ExtraEditAttributes { get; set; }

        /// <summary>
        /// Add a button to the pager that enables the user to add a new line to the grid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public GridPagerControl ShowAdd(bool value)
        {
            Add = value;           
            return this;
        }

        /// <summary>
        /// Adds a new attribute to the add button
        /// </summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="value">The value of the attribute</param>
        /// <returns></returns>
        public GridPagerControl AddAddAttribute(string name, string value)
        {
            ExtraAddAttributes.Add(string.Format("{0}: {1}", name, value));
            return this;
        }

        /// <summary>
        /// Add a button to the pager that enables the user to edit an existing line in the grid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public GridPagerControl ShowEdit(bool value)
        {
            Edit = value;
            return this;
        }

        /// <summary>
        /// Adds a new attribute to the edit button
        /// </summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="value">The value of the attribute</param>
        /// <returns></returns>
        public GridPagerControl AddEditAttribute(string name, string value)
        {
            ExtraEditAttributes.Add(string.Format("{0}: {1}", name, value));
            return this;
        }

        /// <summary>
        /// Add a button to the pager that enables the user to delete an existing line from the grid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="deleteUrl"></param>
        /// <returns></returns>
        public GridPagerControl ShowDel(bool value, string deleteUrl = null)
        {
            Del = value;
            if (deleteUrl != null)
            {
                DeleteUrl = deleteUrl;
            }
            return this;
        }

        /// <summary>
        /// Add a button to the pager that enables the user to view the entire object a grid row is bound to
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public GridPagerControl ShowView(bool value)
        {
            View = value;
            return this;
        }

        /// <summary>
        /// Add a button to the pager that enables the user to search the grid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public GridPagerControl ShowSearch(bool value, bool allowMultipleSearch)
        {
            Search = value;
            return this;
        }

        /// <summary>
        /// Add a button to the pager that enables the user to refresh the grid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public GridPagerControl ShowRefresh(bool value)
        {
            Refresh = value;
            return this;
        }

        /// <summary>
        /// Set the action url the grid will use when the user presses the delete button of the pager
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public GridPagerControl SetDeleteUrl(string value)
        {
            DeleteUrl = value;
            return this;
        }

        public GridPagerControl SetAddEditWidth(int? width)
        {
            _addEditWidth = width;            
            return this;
        }

        public GridPagerControl SetAddEditHeight(int? height)
        {
            _addEditHeight = height;
            return this;
        }
        
        /// <summary>
        /// If search is enabled, will allow the user to search the grid using multiple filters
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public GridPagerControl SetMultipleSearch(bool value)
        {
            this.MultipleSearch = value;
            return this;
        }

        /// <summary>
        /// Is executed after a user presses add, edit or delete
        /// </summary>
        /// <param name="value">The javascript you wish to execute after submit</param>
        /// <returns></returns>
        public GridPagerControl SetAfterSubmit(string value)
        {
            AfterSubmit = value;
            return this;
        }

        public GridPagerControl SetHandleMvcResponse()
        {
            SetAfterSubmit("handleMvcResponse");
            return this;
        }


        public string OnGridLoad()
        {
            return "";
        }

        public string RenderElement()
        {
            return "<div id=\"" + this.Name + "\"></div>";
        }

        public string RenderScript()
        {
            string result =
                "$('#" + this.GridName + "').navGrid('#" +
                this.Name +
                "',{add: " + this.Add.ToString().ToLower() +
                ", edit:" + this.Edit.ToString().ToLower() +
                ", del: " + this.Del.ToString().ToLower() +
                ", search: " + this.Search.ToString().ToLower() +
                ", view: " + this.View.ToString().ToLower() +
                ", refresh: " + this.Search.ToString().ToLower() +

                "},{" + EditParameters() + "}, {" + AddParameters() + "}, {" + DeleteParameters() + "}, ";

            result += "{multipleSearch:" + this.MultipleSearch.ToString().ToLower() + "});";

            return result;
            // prmEdit, prmAdd, prmDel, prmSearch, prmView
        }

        private string EditParameters()
        {
            List<string> parameters = new List<string>(ExtraEditAttributes);
            if (AfterSubmit != null)
            {
                parameters.Add("afterSubmit:" + AfterSubmit);
            }

            AddHeightAndWidth(parameters);

            return string.Join(",", parameters);
        }

        private object AddParameters()
        {
            List<string> parameters = new List<string>(ExtraAddAttributes);
            if (AfterSubmit != null)
            {
                parameters.Add("afterSubmit:" + AfterSubmit);
            }

            AddHeightAndWidth(parameters);

            return string.Join(",", parameters);
        }
        
        private void AddHeightAndWidth(List<string> parameters)
        {
            if (_addEditWidth.HasValue)
            {
                parameters.Add("width:" + _addEditWidth.Value.ToString());
            }
            if (_addEditHeight.HasValue)
            {
                parameters.Add("height:" + _addEditHeight.Value.ToString());
            }
        }        

        private string DeleteParameters()
        {
            List<string> parameters = new List<string>();
            if (DeleteUrl != null)
            {
                parameters.Add("url:\"" + DeleteUrl + "\"");
            }

            if (AfterSubmit != null)
            {
                parameters.Add("afterSubmit:" + AfterSubmit);
            }
            return string.Join(",", parameters);
        }
    }
}
