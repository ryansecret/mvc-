using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Hsr.Models;

namespace Hsr.Model.CustomValidators
{
    public class RegularValidator:IValidator
    {
        Regex regex;
        public bool IsValid(CellValidatorContext context)
        {
            if (Param.Count() != 1)
            { 
                 throw new ArgumentException("{0}缺少正则表达式参数",context.BelongColumn.Tempalecolname);
            }
            regex = new Regex(Param[0].Value);
            if (context.Value != null && !regex.IsMatch(context.Value.ToString()))
            {
                ErrorMessage = string.Format("{0}格式错误",context.BelongColumn.Tempalecolname);
                return false;
            }
            return true;
        }

        public DatamappingParam[] Param { get; set; }

        public string ErrorMessage { get; set; }
    }
}
