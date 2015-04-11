using FluentValidation;
using Hsr.Model.Models;
using Hsr.Models;

namespace Hsr.Model.Validators
{
    public class RyanValidator : AbstractValidator<Ryan>
    {
        public RyanValidator()
        {
            RuleFor(d => d.Name).NotEmpty().Length(5,20);
        }
    }
}