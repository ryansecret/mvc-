#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hsr.Data;
using Hsr.Data.CustomAttribute;

#endregion

namespace Hsr.Models
{
    [Table("POLL_PLAN_DAIWEI", Schema = "MNCMS_APP")]
    public class PollPlanDaiwei : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [KeyGenerate(KeyKind = KeyKind.Guid)]
        [Column("ID")]
        public string Id { get; set; }

        [Column("DAIWEI_COMPANYID")]
        public string DaiweiCompanyid { get; set; }

        [Column("CTUNI_PLAN_ID")]
        public string CtuniPlanId { get; set; }

        [Column("ONEDAY_MAX")]
        public decimal? OnedayMax { get; set; }

        [Column("CREATE_USERID")]
        public string CreateUserid { get; set; }

        [Column("CREATE_TIME")]
        public DateTime  CreateTime { get; set; }

        [Column("LASTEDIT_USERID")]
        public string LasteditUserid { get; set; }

        [Column("LASTEDIT_TIME")]
        public DateTime? LasteditTime { get; set; }

        [Column("POLL_STATUS")]
        public decimal? PollStatus { get; set; }

        [Column("TOTAL_ORDERS")]
        public decimal? TotalOrders { get; set; }

        [Column("DOWN_ORDERS")]
        public decimal? DownOrders { get; set; }

        [Column("FINISH_ORDERS")]
        public decimal? FinishOrders { get; set; }

        [Column("TOTAO_ACTOR")]
        public decimal? TotaoActor { get; set; }

        [Column("TOTAL_FINE")]
        public decimal? TotalFine { get; set; }

        [Column("TOTAL_MIDDLE")]
        public decimal? TotalMiddle { get; set; }

        [Column("TOTAL_BAD")]
        public decimal? TotalBad { get; set; }

        [Column("DAIWEI_COMPANYNAME")]
        public string DaiweiCompanyname { get; set; }

        [Column("MSG_RECEIVE_USERID")]
        public string MsgReceiveUserid { get; set; }

        [Column("APPROVAL_DESC")]
        public string ApprovalDesc { get; set; }

        [Column("APPROVAL_USERID")]
        public string ApprovalUserid { get; set; }

        [Column("APPROVAL_USERNAME")]
        public string ApprovalUsername { get; set; }

        [Column("APPROVAL_TIME")]
        public DateTime? ApprovalTime { get; set; }

        [Column("MSG_RECEIVE_USERNAME")]
        public string MsgReceiveUsername { get; set; }

        [Column("CREATE_USERNAME")]
        public string CreateUsername { get; set; }

        [Column("LASTEDIT_USERNAME")]
        public string LasteditUsername { get; set; }
    }
}