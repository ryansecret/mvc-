#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Model.Models;

#endregion

namespace Hsr.Models
{
    [Table("USER_ROLE", Schema = "MNCMS_APP")]
    public class UserRole : BaseModel
    {
        [Key]
        [ForeignKey("User")]
        [Column("USER_AUID", Order = 1)]
        public string UserAuid { get; set; }

        [Key]
        [ForeignKey("Role")]
        [Column("ROLE_ID", Order = 2)]
        public decimal? RoleId { get; set; }

        [Column("ROLE_NAME")]
        public string RoleName { get; set; }

        public virtual UserSummaryInfo User { get; set; }

        public virtual Sysrole Role { get; set; }
    }
}