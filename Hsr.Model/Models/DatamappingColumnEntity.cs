#region

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Model.Models;
using MVC.Controls.Paging;

#endregion

namespace Hsr.Models
{
    [Table("DATAMAPPING_COLUMN", Schema = "MNCMS_APP")]
    public class DatamappingColumn : BaseModel
    {
        [Column("ID")]
        public decimal? Id { get; set; }

        [Column("DBCOLNAME")]
        public string Dbcolname { get; set; }

        [Column("TEMPALECOLNAME")]
        public string Tempalecolname { get; set; }

        [Column("DATATYPE")]
        public string Datatype { get; set; }

        [Column("DATAID")]
        public decimal? Dataid { get; set; }
    }

    public class DatamappingColumns : BasePageableModel
    {
        public List<DatamappingColumn> Data { get; set; }
        public TreeNode node { get; set; }
    }
}