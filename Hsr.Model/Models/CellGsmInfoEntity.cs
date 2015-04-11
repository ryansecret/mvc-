#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Data.CustomAttribute;

#endregion

namespace Hsr.Models
{
    [Table("CELL_GSM_INFO", Schema = "MNCMS_APP")]
    public class CellGsmInfo : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [KeyGenerate(KeyKind = KeyKind.Id, SeqenceName = "SEQ_CELL_GSM_INFO")]
        [Column("CELL_ID")]
        public decimal? CellId { get; set; }

        [Column("BCCH")]
        public decimal? Bcch { get; set; }

        [Column("BISC")]
        public decimal? Bisc { get; set; }

        [Column("TCH")]
        public string Tch { get; set; }

        [Column("DELFLAG")]
        public decimal? Delflag { get; set; }
    }
}