using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Controls.Grid
{
    /// <summary>
    /// Renders the column as a combobox (HTML select input)
    /// </summary>
    public class ComboColumnRenderer : IColumnRenderer
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source">Define where the list should be filled from.
        /// Possible values are: 
        /// 1. action url (i.e: Users/CityList) - the Controller action should return a List<KeyValuePair<T,K>> object
        /// 2. jscript function (i.e: getCities()) that should return a string in the following format: key:value;key:value ...
        /// </param>
        public ComboColumnRenderer(string source) { Source = source; }

        public string Source { get; set; }

        public GridColumnModel Column { get; set; }

        public string Render(string gridName)
        {
            string dataEvents = this.Column.renderDataEvents();
            if (!string.IsNullOrEmpty(dataEvents)) dataEvents = ", " + dataEvents;

            string valueCallback = " _grid_fillList(\"" + Source + "\", \"" + gridName + "\")";

            return "stype:'select', searchoptions: { value:" + valueCallback + "} , formatter:'select', edittype: 'select', editoptions: { value: " + valueCallback + dataEvents + "}";
        }
    }
}
