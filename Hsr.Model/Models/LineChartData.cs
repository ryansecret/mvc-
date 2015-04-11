using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsr.Model.Models
{
    public class LineChartData
    {
        public List<string> labels { get; set; }
        public List<LineChartDatasets> datasets { get; set; }
    }
}
