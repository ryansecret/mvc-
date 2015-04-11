#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Data.CustomAttribute;

#endregion

namespace Hsr.Models
{
    [Table("BASE_CELL_INFO", Schema = "MNCMS_APP")]
    public class BaseCellInfo : BaseModel
    {
        [Column("CELL_NAME")]
        public string CellName { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [KeyGenerate(KeyKind = KeyKind.Id, SeqenceName = "SEQ_BASE_CELL_INFO")]
        [Column("CELL_ID")]
        public decimal? CellId { get; set; }

        [Column("LAC")]
        public decimal? Lac { get; set; }

        [Column("CI")]
        public decimal? Ci { get; set; }

        [Column("ELECT_TILT")]
        public decimal? ElectTilt { get; set; }

        [Column("AZIMUTH")]
        public decimal? Azimuth { get; set; }

        [Column("SITE_TYPE")]
        public decimal? SiteType { get; set; }

        [Column("PROVINCE_ID")]
        public decimal? ProvinceId { get; set; }

        [Column("CITY_ID")]
        public decimal? CityId { get; set; }

        [Column("AREA_ID")]
        public decimal? AreaId { get; set; }

        [Column("ATTACH_COMPID")]
        public decimal? AttachCompid { get; set; }

        [Column("ATTACH_COMP")]
        public string AttachComp { get; set; }

        [Column("ATTACH_BUILDING")]
        public string AttachBuilding { get; set; }

        [Column("LONGITUDE")]
        public decimal? Longitude { get; set; }

        [Column("LATITUDE")]
        public decimal? Latitude { get; set; }

        [Column("DEVICE_TYPE")]
        public string DeviceType { get; set; }

        [Column("VENDOR_ID")]
        public string VendorId { get; set; }

        [Column("LAST_UPDATETIME")]
        public DateTime LastUpdatetime { get; set; }

        [Column("LAST_UPDATEUSERID")]
        public decimal? LastUpdateuserid { get; set; }

        [Column("LAST_UPDATEUSER")]
        public string LastUpdateuser { get; set; }

        [Column("MARK")]
        public string Mark { get; set; }

        [Column("ATTACH_LOG")]
        public decimal? AttachLog { get; set; }

        [Column("SITE_ID")]
        public decimal? SiteId { get; set; }

        [Column("DELFLAG")]
        public decimal? Delflag { get; set; }
    }
}