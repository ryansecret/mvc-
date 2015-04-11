using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;
using Hsr.Models;

namespace Hsr.Model.CustomValidators
{
    public interface IValidator
    {
        bool IsValid(CellValidatorContext context);

        DatamappingParam[] Param { get; set; }
        string ErrorMessage { get; set; }
    }

    //public abstract class BaseValidator : IValidator
    //{
    //    public bool IsValid(ColumnValidatorContext context, params DatamappingParam[] param)
    //    {
    //        var parentContext = new ValidationContext(null);
    //        var rule = new PropertyRule(null, x => comparer, null, null, typeof(string), null)
    //        {
    //            PropertyName = context.Column.Tempalecolname
    //        };
    //        var fcontext = new PropertyValidatorContext(parentContext, rule, null);
    //        var result = validator.Validate(fcontext);
    //        if (result.Any())
    //        {
    //            ErrorMessage = result.Select(d => d.ErrorMessage).First();
    //            return false;
    //        }
    //        return true;
    //    }

    //    public abstract 

    //    public string ErrorMessage { get; set; }
    //}
}