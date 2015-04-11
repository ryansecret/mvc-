#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Data.CustomAttribute;

#endregion

namespace Hsr.Models
{
    [Table("POLL_PLAN_DAIWEI_DETAIL", Schema = "MNCMS_APP")]
    public class PollPlanDaiweiDetail : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [KeyGenerate(KeyKind = KeyKind.Guid)]
        [Column("AUID")]
        public string Auid { get; set; }

        [Column("XJ_PLAN_DAIWEIID")]
        public string XjPlanDaiweiid { get; set; }

        [Column("SITE_ID")]
        public string SiteId { get; set; }

        [Column("SITE_NAME")]
        public string SiteName { get; set; }

        [Column("GRADE_LEVEL")]
        public string GradeLevel { get; set; }

        [Column("PLAN_DATE")]
        public DateTime PlanDate { get; set; }

        [Column("REAL_DATE")]
        public DateTime RealDate { get; set; }

        [Column("WORK_ORDERID")]
        public decimal? WorkOrderid { get; set; }

        [Column("POLL_STATUS")]
        public decimal? PollStatus { get; set; }

        [Column("VERNO")]
        public decimal? Verno { get; set; }

        [Column("QUALITY")]
        public decimal? Quality { get; set; }

        [Column("BUILDING_ID")]
        public string BuildingId { get; set; }

        [Column("BUILDING_NAME")]
        public string BuildingName { get; set; }

        [Column("WORK_SUB_GROUPID")]
        public decimal? WorkSubGroupid { get; set; }

        [Column("WORK_NAME")]
        public string WorkName { get; set; }

        [Column("IS_OK")]
        public decimal? IsOk { get; set; }

        [Column("OBSTACLE_USERID")]
        public string ObstacleUserid { get; set; }

        [Column("OBSTACLE_DESC")]
        public string ObstacleDesc { get; set; }

        [Column("OBSTACLE_TIME")]
        public DateTime ObstacleTime { get; set; }

        [Column("IN_OUT")]
        public decimal? InOut { get; set; }

        [Column("POLL_LEVEL")]
        public decimal? PollLevel { get; set; }

        [Column("LAST_TIME")]
        public DateTime LastTime { get; set; }
    }
}