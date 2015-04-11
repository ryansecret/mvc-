#region

using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;

#endregion

namespace Hsr.Models
{
    [Table("PROJECT_GROUP_INFO", Schema = "MNCMS_APP")]
    public class ProjectGroupInfo : BaseModel
    {
        [Column("GROUP_ID")]
        public decimal? GroupId { get; set; }

        [Column("P_GROUP_ID")]
        public decimal? PGroupId { get; set; }

        [Column("GROUP_NAME")]
        public string GroupName { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Column("PROVINCE_ID")]
        public decimal? ProvinceId { get; set; }

        [Column("CITY_ID")]
        public decimal? CityId { get; set; }

        [Column("AREA_ID")]
        public decimal? AreaId { get; set; }

        [Column("CREATE_USERID")]
        public string CreateUserid { get; set; }

        [Column("CREATE_USERNAME")]
        public string CreateUsername { get; set; }
    }
}