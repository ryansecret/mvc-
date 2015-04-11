#region

using System.Collections.Generic;

#endregion

namespace Hsr.Model.Models.ViewModel
{
    public class BarChartData
    {
        public List<string> labels { get; set; }
        public List<BarChartDatasets> datasets { get; set; }
    }
}