using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Hsr.Model.Models;
using Hsr.Model.Models.ViewModel;

namespace Hsr.Model.Validators
{
    public class MenuValidator : AbstractValidator<MenuVm>
    {
        public MenuValidator()
        {
            RuleFor(d => d.OrderNum).NotEmpty();
            RuleFor(d => d.Menuname).NotEmpty();
            RuleFor(d => d.Controller).NotEmpty();
            RuleFor(d => d.Action).NotEmpty();
        }
    }
}
