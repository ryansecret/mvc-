using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsr.Data.Compare.Validators
{
    public class LengthValidator:IValidator
    {
        private string _errorMessasge;

        public bool IsValid(params CompareParameters[] parameters)
        {
            CompareParameters compareParameters = parameters[0];

            if (compareParameters==null||compareParameters.ToString().Length > Length)
            {
                ErrorMessasge = string.Format("{0}长度应该不超过{1}", ValidateRule.Alias, ValidateRule.Value);
                return false;
            }
            return true;
        }

        public int Length { get { return int.Parse(ValidateRule.Value); }}

        public  ValidateRule ValidateRule { get; set; }
        public string ErrorMessasge
        { 
            get { return _errorMessasge; }
            set { _errorMessasge = value; }
        }
    }
}
