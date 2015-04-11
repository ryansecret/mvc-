#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Data.CustomAttribute;

#endregion

namespace Hsr.Models
{
    [Table("BASE_FLOORS_INFO", Schema = "MNCMS_APP")]
    public class BaseFloorsInfo : BaseModel
    {
        [Column("DELFLAG")]
        public decimal? Delflag { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [KeyGenerate(KeyKind = KeyKind.Id, SeqenceName = "SEQ_BASE_FLOORS_INFO")]
        [Column("FLOORS_ID")]
        public int FloorsId { get; set; }

        [Column("BUILDING_ID")]
        public decimal? BuildingId { get; set; }

        [Column("BUILDING_NAME")]
        public string BuildingName { get; set; }

        [Column("FLOORS_TYPE")]
        public decimal? FloorsType { get; set; }

        [Column("GSM_COVER")]
        public decimal? GsmCover { get; set; }

        [Column("TDSCDMA_COVER")]
        public decimal? TdscdmaCover { get; set; }

        [Column("LTE_COVER")]
        public decimal? LteCover { get; set; }

        [Column("FLOOR_GRAPH_PATH")]
        public string FloorGraphPath { get; set; }

        [Column("FLOOR_DESIGN_PATH")]
        public string FloorDesignPath { get; set; }

        [Column("IS_FORBID_STATIS")]
        public decimal? IsForbidStatis { get; set; }

        [Column("MARK")]
        public string Mark { get; set; }

        [Column("LAST_UPDATETIME")]
        public DateTime LastUpdatetime { get; set; }

        [Column("LAST_UPDATEUSERID")]
        public decimal? LastUpdateuserid { get; set; }

        [Column("LAST_UPDATEUSER")]
        public string LastUpdateuser { get; set; }
    }
}