#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Data.CustomAttribute;
using Hsr.Models;

#endregion

namespace Hsr.Model.Models
{
    [Table("SYSROLE", Schema = "MNCMS_APP")]
    public class Sysrole : BaseModel
    {
         [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [KeyGenerate(KeyKind = KeyKind.Id, SeqenceName = "SEQ_SYSROLE")]
        [Column("ROLE_ID")]
        public decimal? RoleId { get; set; }

        [Column("ROLE_NAME")]
        public string RoleName { get; set; }

        [Column("CATEGORY")]
        public decimal? Category { get; set; }

        [Column("ISENABLED")]
        public decimal? Isenabled { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Column("CREATE_TIME")]
        public DateTime CreateTime { get; set; }

        [Column("CREATE_BY")]
        public string CreateUserid { get; set; }

        [Column("CREATE_USERID")]
        public string CreateBy { get; set; }

        [Column("ROLE_PID")]
        public decimal? RolePid { get; set; }

        
    }
}