#region

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Model.Models;
using MVC.Controls.Paging;

#endregion

namespace Hsr.Models
{
    [Table("DATAMAPPING", Schema = "MNCMS_APP")]
    public class Datamapping : BaseModel
    {
        [Column("ID")]
        public decimal? Id { get; set; }

        [Column("DBTABLE")]
        public string Dbtable { get; set; }

        [Column("NICKNAME")]
        public string Nickname { get; set; }
    }


    public class Datamappings : BasePageableModel
    {
        public List<Datamapping> Data { get; set; }
        public TreeNode node { get; set; }
    }
}