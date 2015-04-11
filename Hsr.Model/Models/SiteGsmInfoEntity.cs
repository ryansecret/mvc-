#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Data.CustomAttribute;

#endregion

namespace Hsr.Models
{
    [Table("SITE_GSM_INFO", Schema = "MNCMS_APP")]
    public class SiteGsmInfo : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [KeyGenerate(KeyKind = KeyKind.Id, SeqenceName = "SEQ_SITE_GSM_INFO")]
        [Column("SITE_ID")]
        public decimal? SiteId { get; set; }

        [Column("BCCH")]
        public decimal? Bcch { get; set; }

        [Column("BISC")]
        public decimal? Bisc { get; set; }

        [Column("TCH")]
        public string Tch { get; set; }

        [Column("ELECT_TILT")]
        public decimal? ElectTilt { get; set; }

        [Column("DELFLAG")]
        public decimal? Delflag { get; set; }
    }
}