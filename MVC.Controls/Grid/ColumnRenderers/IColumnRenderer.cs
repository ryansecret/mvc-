using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Controls.Grid
{
    /// <summary>
    /// Allows for a custome column editing rendering mechanisms
    /// </summary>
    public interface IColumnRenderer
    {
        string Render(string gridName); 
        GridColumnModel Column { get; set; } 
    }
}
