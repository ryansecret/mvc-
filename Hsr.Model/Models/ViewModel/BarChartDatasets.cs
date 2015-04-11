#region

using System.Collections.Generic;

#endregion

namespace Hsr.Model.Models.ViewModel
{
    public class BarChartDatasets
    {
        public string fillColor { get; set; }
        public string strokeColor { get; set; }
        public List<float> data { get; set; }
    }
}