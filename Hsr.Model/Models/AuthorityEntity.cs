#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Data.CustomAttribute;

#endregion

namespace Hsr.Models
{
    [Table("AUTHORITY", Schema = "MNCMS_APP")]
    public class Authority : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [KeyGenerate(KeyKind = KeyKind.Id, SeqenceName = "SEQ_AUTHORITY")]
        [Column("ID")]
        public int? Id { get; set; }

        [Column("MENUID")]
        public decimal? Menuid { get; set; }

        [Column("ROLE_ID")]
        public decimal? RoleId { get; set; }

        [NotMapped]
        public string MenuIds { get; set; }
    }
}