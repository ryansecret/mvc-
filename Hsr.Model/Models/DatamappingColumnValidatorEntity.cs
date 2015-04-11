#region

using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;

#endregion

namespace Hsr.Models
{
    [Table("DATAMAPPING_COLUMN_VALIDATOR", Schema = "MNCMS_APP")]
    public class DatamappingColumnValidator : BaseModel
    {
        [Column("COLUMNID")]
        public decimal? Columnid { get; set; }

        [Column("VALIDATORID")]
        public decimal? Validatorid { get; set; }
    }
}