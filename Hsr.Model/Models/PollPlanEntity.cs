#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Data.CustomAttribute;

#endregion

namespace Hsr.Models
{
    [Table("POLL_PLAN", Schema = "MNCMS_APP")]
    public class PollPlan : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [KeyGenerate(KeyKind = KeyKind.Guid)]
        [Column("AUID")]
        public string Auid { get; set; }

        [Column("PROVINCE_ID")]
        public decimal? ProvinceId { get; set; }

        [Column("CITY_ID")]
        public decimal? CityId { get; set; }

        [Column("NET_TYPE")]
        public decimal? NetType { get; set; }

        [Column("IN_OUT")]
        public decimal? InOut { get; set; }

        [Column("YEAR")]
        public decimal? Year { get; set; }

        [Column("PLAN_STATUS")]
        public decimal? PlanStatus { get; set; }

        [Column("CREATE_USERID")]
        public string CreateUserid { get; set; }

        [Column("CREATE_TIME")]
        public DateTime CreateTime { get; set; }

        [Column("LASTEDIT_USERID")]
        public string LasteditUserid { get; set; }

        [Column("LASTEDIT_TIME")]
        public DateTime LasteditTime { get; set; }

        [Column("BEGIN_DATE")]
        public DateTime BeginDate { get; set; }

        [Column("END_DATE")]
        public DateTime EndDate { get; set; }

        [Column("PROJECT_ID")]
        public decimal? ProjectId { get; set; }

        [Column("AREA_ID")]
        public decimal? AreaId { get; set; }

        [Column("LASTEDIT_USERNAME")]
        public string LasteditUsername { get; set; }

        [Column("PROJECT_NAME")]
        public string ProjectName { get; set; }

        [Column("PRO_CATEGORY")]
        public decimal? ProCategory { get; set; }

        [Column("PLAN_ID")]
        public decimal? PlanId { get; set; }

        [Column("PLAN_NAME")]
        public string PlanName { get; set; }

        [Column("WORK_TYPE")]
        public decimal? WorkType { get; set; }

        [Column("LAST_WORKORDER_DATE")]
        public DateTime LastWorkorderDate { get; set; }

        [Column("CREATE_USERNAME")]
        public string CreateUsername { get; set; }

        [Column("NAME")]
        public string Name { get; set; }
    }
}