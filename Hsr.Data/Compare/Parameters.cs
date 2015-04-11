using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsr.Data.Compare
{
    public class CompareParameters
    {
        public string Type { get; set; }

        public string Value { get; set; }
    }

    public class ValidateRule
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public string Alias { get; set; }
    }
}
