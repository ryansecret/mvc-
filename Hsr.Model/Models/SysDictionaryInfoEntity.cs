#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Data.CustomAttribute;

#endregion

namespace Hsr.Models
{
    [Table("SYS_DICTIONARY_INFO", Schema = "MNCMS_APP")]
    public class SysDictionaryInfo : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("CODE_ID")]
        public decimal? CodeId { get; set; }

        [Column("CHNAME")]
        public string Chname { get; set; }

        [Column("ENNAME")]
        public string Enname { get; set; }

        [Column("GROUP_NAME")]
        public string GroupName { get; set; }

        [Column("REMARKS")]
        public string Remarks { get; set; }

        
 
        [Column("SORT")]
        public decimal? Sort { get; set; }
    }
}