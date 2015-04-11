#region

using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Data.CustomAttribute;

#endregion

namespace Hsr.Model.Models
{
    [Table("MENU", Schema = Schema)]
    public class Menu : BaseModel 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [KeyGenerate(KeyKind = KeyKind.Id, SeqenceName = "SEQ_MENU")]
        [Column("ID")]
        public int? Id { get; set; }

        [Column("MENUNAME")]
        public string Menuname { get; set; }

        [Column("PID")]
        public decimal? Pid { get; set; }

        [Column("PMENUNAME")]
        public string PmenuName { get; set; }

        [Column("LAYER")]
        public decimal? Layer { get; set; }

        [Column("ISENABLED")]
        public decimal Isenabled { get; set; }

        [Column("ACTION")]
        public string Action { get; set; }

        [Column("CONTROLLER")]
        public string Controller { get; set; }

        [Column("ICON")]
        public string Icon { get; set; }

        [Column("ORDER_NUM")]
        public decimal? OrderNum { get; set; }
        [NotMapped]
        public string ForbiddenAction { get; set; }

      
    }
}