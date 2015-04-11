using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Hsr.Model.Models.ViewModel;

namespace Hsr.Model.Validators
{
    public class RoleValidator : AbstractValidator<RoleVm>
    {
        public RoleValidator()
        {
            RuleFor(d => d.RoleName).NotEmpty().WithMessage("请输入");
        }
    }
}
