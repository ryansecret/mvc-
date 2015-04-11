using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hsr.Models;

namespace Hsr.Model.CustomValidators
{
    public class CellValidatorContext
    {
         public DatamappingColumn BelongColumn { get; set; }
         public object Value { get; set; }
         
         public List<IValidator>  Validators { get; set; }
         
         public IEnumerable<string> ErrorMessages{
             get { return Validators.Select(d => d.ErrorMessage); }
         }

        public CellValidatorContext Clone()
        {
            return new CellValidatorContext(){BelongColumn =this.BelongColumn,Validators = this.Validators};
        }
    }

    
}
