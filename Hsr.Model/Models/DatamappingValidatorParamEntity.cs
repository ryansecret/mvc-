#region

using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;

#endregion

namespace Hsr.Models
{
    [Table("DATAMAPPING_VALIDATOR_PARAM", Schema = "MNCMS_APP")]
    public class DatamappingValidatorParam : BaseModel
    {
        [Column("PARAMID")]
        public decimal? Paramid { get; set; }

        [Column("VALIDATORID")]
        public decimal? Validatorid { get; set; }
    }
}