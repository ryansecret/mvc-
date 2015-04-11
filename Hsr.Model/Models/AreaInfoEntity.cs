#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Data.CustomAttribute;

#endregion

namespace Hsr.Model.Models
{
    [Table("AREA_INFO", Schema = "MNCMS_APP")]
    public class AreaInfo : BaseModel
    {
        [Key]
        [KeyGenerate(KeyKind = KeyKind.Id)]
        [Column("AREA_ID")]
        public decimal? AreaId { get; set; }

        [Column("AREA_NAME")]
        public string AreaName { get; set; }

        [Column("CITY_ID")]
        public decimal? CityId { get; set; }

        [Column("CITY_NAME")]
        public string CityName { get; set; }

        [Column("PROVINCE_ID")]
        public decimal? ProvinceId { get; set; }

        [Column("PROVINCE_NAME")]
        public string ProvinceName { get; set; }

        [Column("PROVINCE_NAME_STR")]
        public string ProvinceNameStr { get; set; }

        [Column("AREA_CID")]
        public decimal? AreaCid { get; set; }

        [Column("CITY_CID")]
        public decimal? CityCid { get; set; }

        [Column("PROVINCE_CID")]
        public decimal? ProvinceCid { get; set; }
    }
}