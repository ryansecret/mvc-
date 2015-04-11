using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Controls.Grid
{
    public interface IGridController
    {
        IEnumerable<GridColumnModel> GetRawColumns();
        string GetEditUrl();
        string GetListUrl();
        string GetEditUrl(object parent);
        string GetListUrl(object parent);
    }
}
