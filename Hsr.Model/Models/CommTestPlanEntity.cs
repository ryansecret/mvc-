#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Hsr.Data;
using Hsr.Data.CustomAttribute;
using MVC.Controls.Paging;

#endregion

namespace Hsr.Models
{
    [Table("COMM_TEST_PLAN", Schema = "MNCMS_APP")]
    public class CommTestPlan : BaseModel
    {
        private string _isEnable;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [KeyGenerate(KeyKind = KeyKind.Id, SeqenceName = "SEQ_COMM_TEST_PLAN")]
        [Column("PLAN_ID")]
        public int PlanId { get; set; }

        [Column("PLAN_NAME")]
        [Display(Name ="方案名称")]
        [Required]
        public string PlanName { get; set; }

        [Column("PROVINCE_ID")]
        public decimal? ProvinceId { get; set; }

        [Column("CITY_ID")]
        public decimal? CityId { get; set; }

        [Column("AREA_ID")]
        public decimal? AreaId { get; set; }

        [Column("CREATE_TIME")]
        [Display(Name ="编辑时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode = false)]
        public DateTime? CreateTime { get; set; }

        [Column("CREATE_USERID")]
        public string CreateUserid { get; set; }

        [Column("CREATE_USERNAME")]
        [Display(Name ="编辑用户")]
        public string CreateUsername { get; set; }

        [Column("IS_ENABLED")]
        public decimal? IsEnabled { get; set; }

        [Column("COMMENTS")]
        [Display(Name ="备注")]
        public string Comments { get; set; }

        [Column("LOOP_SUM")]
        [Display(Name ="执行次数")]
        [Required]
        public decimal? LoopSum { get; set; }

        [Column("LOOP_INTERVAL")]
        [Display(Name = "执行间隔")]
        [Required]
        public decimal? LoopInterval { get; set; }

        [Column("PLAN_TYPE")]
        [Display(Name ="计划类型")]
        [Required]
         
        public decimal? PlanType { get; set; }
        [NotMapped]
        public string PlayTypeName { get; set; }

        [NotMapped]
        [Required]
        [Display(Name = "状态")]
        public bool IsEnable
        {
            get { return IsEnabled==1; }
            set { IsEnabled = value ? 1 : 0; }
        } 
    }

    public class ComTestPlanPage:BasePageableModel
    {
        public List<CommTestPlan> Data { get; set; } 
    }
}