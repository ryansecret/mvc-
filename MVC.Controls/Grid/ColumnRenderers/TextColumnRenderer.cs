using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Controls.Grid
{
    /// <summary>
    /// The default column editing renderer
    /// </summary>
    public class TextColumnRenderer : IColumnRenderer
    {
        public string Render(string gridName)
        {
            string dataEvents = this.Column.renderDataEvents();
            if (!string.IsNullOrEmpty(dataEvents))
            {
                dataEvents = ", editoptions: {" + dataEvents + "}";
            }

            return "edittype: 'text'" + dataEvents;
        }
        public GridColumnModel Column { get; set; }
    }
}
