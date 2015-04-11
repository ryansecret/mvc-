using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hsr.Models;

namespace Hsr.Model.CustomValidators
{
    public class LengthValidator:IValidator
    {
        public bool IsValid(CellValidatorContext context)
        {
            if (Param.Length != 2)
            {
                 throw new ArgumentException("长度区间验证器参数应该为2个");
            }
            int min, max;
            try
            {
                min = Convert.ToInt16(Param[0]);
                max = Convert.ToInt16(Param[1]);
            }
            catch (Exception)
            {
                throw new ArgumentException("长度区间验证器");
            }
            if (string.IsNullOrWhiteSpace(context.Value.ToString()))
            {
                return false;
            }
            var length = context.Value.ToString().Length;
            if (length <= max && length>min)
            {
                return true;
            }
            ErrorMessage = string.Format("{0}长度应该在{1}与{2}之间",context.BelongColumn.Tempalecolname,min,max);
            return false;
        }

        public DatamappingParam[] Param { get; set; }

        public string ErrorMessage { get; set; }
    }
}
