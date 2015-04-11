#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Data.CustomAttribute;

#endregion

namespace Hsr.Model.Models
{
    [Table("ORGANIZE_INFO", Schema = "MNCMS_APP")]
    public class OrganizeInfo : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [KeyGenerate(KeyKind = KeyKind.Id, SeqenceName = "SEQ_ORGANIZE_INFO")]
        [Column("ORG_ID")]
        public decimal? OrgId { get; set; }

        [Column("CATEGORY")]
        public string Category { get; set; }

        [Column("LOGO")]
        public string Logo { get; set; }

        [Column("AREA_ID")]
        public decimal? AreaId { get; set; }

        [Column("CITY_ID")]
        public decimal? CityId { get; set; }

        [Column("PROVINCE_ID")]
        public decimal? ProvinceId { get; set; }

        [Column("ISENABLED")]
        public decimal? Isenabled { get; set; }

        [Column("CREATE_BY")]
        public string CreateBy { get; set; }

        [Column("CREATE_USERID")]
        public string CreateUserid { get; set; }

        [Column("CREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Column("ADDRESS")]
        public string Address { get; set; }

        [Column("POST_CODE")]
        public decimal? PostCode { get; set; }

        [Column("FAX")]
        public string Fax { get; set; }

        [Column("PHONE")]
        public string Phone { get; set; }

        [Column("ORG_PID")]
        public decimal? OrgPid { get; set; }

        [Column("ORG_NAME")]
        public string OrgName { get; set; }
        [Column("ORG_TYPE")]
        public decimal? OrgType { get; set; }
    }
}