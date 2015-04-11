#region

using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;

#endregion

namespace Hsr.Models
{
    [Table("DATAMAPPING_VALIDATOR", Schema = "MNCMS_APP")]
    public class DatamappingValidator : BaseModel
    {
        [Column("ID")]
        public decimal? Id { get; set; }

        [Column("REGISTERNAME")]
        public string Registername { get; set; }

        [Column("NICKNAME")]
        public string Nickname { get; set; }
    }
}