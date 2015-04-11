using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hsr.Models;

namespace Hsr.Model.CustomValidators
{
    public  class NotEmptyValidator:IValidator
    {
        private string _errorMessage="";

        public bool IsValid(CellValidatorContext context)
        {
            if (string.IsNullOrWhiteSpace(context.Value.ToString()))
            {
                return true;
            }
            ErrorMessage = string.Format("{0}不能为空", context.BelongColumn.Tempalecolname);
            return false;
        }

        public DatamappingParam[] Param { get; set; }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
    }
}
