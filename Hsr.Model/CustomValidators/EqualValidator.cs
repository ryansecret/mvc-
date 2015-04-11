using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;
using Hsr.Models;

namespace Hsr.Model.CustomValidators
{
    public  class EqualValidator:IValidator
    {
        public bool IsValid(CellValidatorContext context)
        {
            if (Param[0].Type == "string")
            {
                return context.Value.ToString() == Param[0].Value;
            }
            var comparer = Convert.ToDouble(Param[0].Value);
            var comparee = Convert.ToDouble(context.Value);
            if (comparee!=comparer)
            {
                ErrorMessage = string.Format("{0}不等于{1}", context.BelongColumn.Tempalecolname, Param[0].Value);
            }
            return comparer == comparee;
        }
 
        public DatamappingParam[] Param { get; set; }
        public string ErrorMessage { get; set; }
    }
}
