#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Data.CustomAttribute;

#endregion

namespace Hsr.Models
{
    [Table("CELL_TDSCDMA_INFO", Schema = "MNCMS_APP")]
    public class CellTdscdmaInfo : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [KeyGenerate(KeyKind = KeyKind.Id, SeqenceName = "SEQ_CELL_TDSCDMA_INFO")]
        [Column("CELL_ID")]
        public decimal? CellId { get; set; }

        [Column("PSC")]
        public decimal? Psc { get; set; }

        [Column("RNC")]
        public string Rnc { get; set; }

        [Column("CHANNE_UI")]
        public decimal? ChanneUi { get; set; }

        [Column("UARFCN")]
        public decimal? Uarfcn { get; set; }

        [Column("DELFLAG")]
        public decimal? Delflag { get; set; }
    }
}