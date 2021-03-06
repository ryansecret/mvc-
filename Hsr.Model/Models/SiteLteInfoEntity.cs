﻿#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Data.CustomAttribute;

#endregion

namespace Hsr.Models
{
    [Table("SITE_LTE_INFO", Schema = "MNCMS_APP")]
    public class SiteLteInfo : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [KeyGenerate(KeyKind = KeyKind.Id, SeqenceName = "SEQ_SITE_LTE_INFO")]
        [Column("SITE_ID")]
        public decimal? SiteId { get; set; }

        [Column("PCI")]
        public decimal? Pci { get; set; }

        [Column("PSC")]
        public decimal? Psc { get; set; }

        [Column("UARFCN")]
        public decimal? Uarfcn { get; set; }

        [Column("CHANNE_UI")]
        public decimal? ChanneUi { get; set; }

        [Column("DELFLAG")]
        public decimal? Delflag { get; set; }
    }
}