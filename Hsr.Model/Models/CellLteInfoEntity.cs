#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Data.CustomAttribute;

#endregion

namespace Hsr.Models
{
    [Table("CELL_LTE_INFO", Schema = "MNCMS_APP")]
    public class CellLteInfo : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [KeyGenerate(KeyKind = KeyKind.Id, SeqenceName = "SEQ_CELL_LTE_INFO")]
        [Column("CELL_ID")]
        public decimal? CellId { get; set; }

        [Column("PCI")]
        public decimal? Pci { get; set; }

        [Column("UARFCN")]
        public decimal? Uarfcn { get; set; }

        [Column("CHANNE_UI")]
        public decimal? ChanneUi { get; set; }

        [Column("DELFLAG")]
        public decimal? Delflag { get; set; }
    }
}