using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;
using Hsr.Models;

namespace Hsr.Model.Validators
{
    public class DetailInfoValidator : AbstractValidator<UserDetailInfo>
    {
        public DetailInfoValidator()
        {
            
            RuleFor(d => d.Mobile).NotNull().Matches(@"^1[3|5|7|8|][0-9]{9}$").WithMessage("请输入正确的手机号");
            RuleFor(d => d.RealName).NotEmpty();
            RuleFor(d => d.Email).EmailAddress().NotEmpty();
            RuleFor(d => d.CompId).NotNull();
            RuleFor(d => d.DeptId).NotEmpty();

            //RuleFor(d => d.Telphone).Matches(@"^\d{3}\d{8}|\d{4}\d{7}$").WithMessage("请输入正确的电话格式");
;        }
          
    }
}
