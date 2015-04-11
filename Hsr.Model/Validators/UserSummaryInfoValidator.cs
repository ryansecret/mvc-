using FluentValidation;
using Hsr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsr.Model.Validators
{
    public class UserSummaryInfoValidator : AbstractValidator<UserSummaryInfo>
    {
        public UserSummaryInfoValidator()
        {
            RuleFor(d => d.RoleIds).NotNull().WithMessage("请选择角色");

            RuleFor(d => d.UserName).NotNull().WithMessage("请输入用户名");
            RuleFor(d => d.UserPw).NotNull().WithMessage("请输入密码").Length(6, 32).WithMessage(
                               "密码长度在6-32位");
            RuleFor(d => d.UserPwCompare).NotNull().Equal(d => d.UserPw).WithMessage("两次密码输入不一致！");

            RuleSet("create",
                    () =>
                        { 
                            
                            RuleFor(d => d.UserPw).NotNull().WithMessage("请输入密码").Length(6, 32).WithMessage(
                                "密码长度在6-32位");
                            RuleFor(d => d.UserPwCompare).NotNull().Equal(d => d.UserPw).WithMessage("两次密码输入不一致！");
                        });
            RuleSet("Edit",
                   () => RuleFor(d => d.UserName).NotNull().WithMessage("请输入用户名"));

        }
    }
}
