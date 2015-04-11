#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;

#endregion

namespace Hsr.Models
{
    [Table("DATA_RULE", Schema = "MNCMS_APP")]
    public class DataRule : BaseModel
    {
        [Column("PROVINCE_ID")]
        [Key]
        public decimal? ProvinceId { get; set; }

        [Column("NODE_ID")]
        [ForeignKey("NodeInfo")]
        public decimal? NodeId { get; set; }

        public virtual DataNodeInfo NodeInfo { get; set; }
        [Column("IS_ENABLED")]
        public decimal? IsEnabled { get; set; }
    }
}