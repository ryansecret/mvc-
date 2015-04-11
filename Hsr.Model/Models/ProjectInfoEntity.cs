#region

using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;

#endregion

namespace Hsr.Models
{
    [Table("PROJECT_INFO", Schema = "MNCMS_APP")]
    public class ProjectInfo : BaseModel
    {
        [Column("PROJECT_ID")]
        public decimal? ProjectId { get; set; }

        [Column("PROJECT_NAME")]
        public string ProjectName { get; set; }

        [Column("CATEGORY")]
        public decimal? Category { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Column("SENDER")]
        public string Sender { get; set; }

        [Column("CREATE_TIME")]
        public decimal? CreateTime { get; set; }

        [Column("CREATE_USERID")]
        public string CreateUserid { get; set; }

        [Column("CREATE_BY")]
        public string CreateBy { get; set; }

        [Column("RESP_PEOPLE")]
        public string RespPeople { get; set; }

        [Column("RESP_EMAIL")]
        public string RespEmail { get; set; }

        [Column("PROVINCE_ID")]
        public decimal? ProvinceId { get; set; }

        [Column("CITY_ID")]
        public decimal? CityId { get; set; }

        [Column("AREA_ID")]
        public decimal? AreaId { get; set; }

        [Column("DATE_BEGIN")]
        public decimal? DateBegin { get; set; }

        [Column("DATE_END")]
        public decimal? DateEnd { get; set; }

        [Column("PRO_STATE")]
        public decimal? ProState { get; set; }

        [Column("DSTART_TIME")]
        public decimal? DstartTime { get; set; }

        [Column("DEND_TIME")]
        public decimal? DendTime { get; set; }

        [Column("PROJECT_GID")]
        public decimal? ProjectGid { get; set; }

        [Column("IN_OUT")]
        public decimal? InOut { get; set; }

        [Column("PROJ_TYPE")]
        public decimal? ProjType { get; set; }

        [Column("ISCOUNT")]
        public decimal? Iscount { get; set; }
    }
}