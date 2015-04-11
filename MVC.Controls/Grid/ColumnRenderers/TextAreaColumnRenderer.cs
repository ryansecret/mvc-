using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Controls.Grid
{
    /// <summary>
    /// 
    /// </summary>
    public class TextAreaColumnRenderer : IColumnRenderer
    {
        public TextAreaColumnRenderer(int rows, int cols)        
        {
            Rows = rows;
            Cols = cols;
        }

        public string Render(string gridName)
        {
            string dataEvents = this.Column.renderDataEvents();
            string rowcol = string.Format("rows:\"{0}\", cols:\"{1}\"", Rows, Cols);
            if (!string.IsNullOrEmpty(dataEvents))
            {
                dataEvents = ", editoptions: {" + dataEvents + "," + rowcol + "}";
            }
            else
            {
                dataEvents = ", editoptions: {" + rowcol + "}";
            }


            return "edittype: 'textarea'" + dataEvents;
        }
        public GridColumnModel Column { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
    }
}
