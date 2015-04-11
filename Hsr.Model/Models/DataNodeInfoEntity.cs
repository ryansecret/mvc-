#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;

#endregion

namespace Hsr.Models
{
    [Table("DATA_NODE_INFO", Schema = "MNCMS_APP")]
    public class DataNodeInfo : BaseModel
    {
        [Column("NODE_ID")]
        [Key]
        public decimal? NodeId { get; set; }

        [Column("NODE_NAME")]
        public string NodeName { get; set; }

        [Column("NODE_IP")]
        public string NodeIp { get; set; }

        [Column("CIFS_ACCOUT")]
        public string CifsAccout { get; set; }

        [Column("CIFS_PASSWD")]
        public string CifsPasswd { get; set; }

        [Column("CIFS_DIR")]
        public string CifsDir { get; set; }

        [Column("NFS_DIR")]
        public string NfsDir { get; set; }
    }
}