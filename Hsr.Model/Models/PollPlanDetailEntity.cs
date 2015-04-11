#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Data.CustomAttribute;

#endregion

namespace Hsr.Models
{
    [Table("POLL_PLAN_DETAIL", Schema = "MNCMS_APP")]
    public class PollPlanDetail : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [KeyGenerate(KeyKind = KeyKind.Guid)]
        [Column("AUID")]
        public string Auid { get; set; }

        [Column("SITE_ID")]
        public string SiteId { get; set; }

        [Column("SITE_NAME")]
        public string SiteName { get; set; }

        [Column("BUILDING_ID")]
        public string BuildingId { get; set; }

        [Column("BUILDING_NAME")]
        public string BuildingName { get; set; }

        [Column("GRADE_LEVEL")]
        public decimal? GradeLevel { get; set; }

        [Column("DAIWEI_COMPANYID")]
        public string DaiweiCompanyid { get; set; }

        [Column("XJ_BEGIN_DATE")]
        public DateTime XjBeginDate { get; set; }

        [Column("QUALITY")]
        public decimal? Quality { get; set; }

        [Column("PLAN_ID")]
        public string PlanId { get; set; }

        [Column("OBSTACLE")]
        public decimal? Obstacle { get; set; }

        [Column("DESIGN_CHANGE")]
        public decimal? DesignChange { get; set; }

        [Column("SCHEDULE_STATUS")]
        public decimal? ScheduleStatus { get; set; }

        [Column("POLL_LEVEL")]
        public decimal? PollLevel { get; set; }

        [Column("DAIWEI_COMPANYNAME")]
        public string DaiweiCompanyname { get; set; }

        [Column("POLL_LEVEL_NAME")]
        public string PollLevelName { get; set; }

        [Column("IN_OUT")]
        public decimal? InOut { get; set; }

        [Column("IS_NEW_UNIT")]
        public decimal? IsNewUnit { get; set; }
    }
}