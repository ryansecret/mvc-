#region

using System.Collections.Generic;
using Hsr.Models;

#endregion

namespace Hsr.Model.Models.ViewModel
{
    public class DataColumnEx : DatamappingColumn
    {
        public List<ValidatorEx> Validators { get; set; }
    }

    public class ValidatorEx : DatamappingValidator
    {
        public List<DatamappingParam> Params { get; set; }
    }
}