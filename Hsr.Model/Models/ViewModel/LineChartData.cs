#region

using System.Collections.Generic;

#endregion

namespace Hsr.Model.Models.ViewModel
{
    public class LineChartData
    {
        public List<string> labels { get; set; }
        public List<LineChartDatasets> datasets { get; set; }
    }
}