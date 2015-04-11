using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsr.Data.Compare
{
    public  interface IValidator
    {
        bool IsValid(params CompareParameters[] parameters);
        
        string ErrorMessasge { get; set; }
    }
}
