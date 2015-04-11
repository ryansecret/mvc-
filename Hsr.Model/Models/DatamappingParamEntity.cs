#region

using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;

#endregion

namespace Hsr.Models
{
    [Table("DATAMAPPING_PARAM", Schema = "MNCMS_APP")]
    public class DatamappingParam : BaseModel
    {
        [Column("ID")]
        public decimal? Id { get; set; }

        [Column("TYPE")]
        public string Type { get; set; }

        [Column("VALUE")]
        public string Value { get; set; }
    }
}