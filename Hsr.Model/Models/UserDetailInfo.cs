#region

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation.Attributes;
using Hsr.Data;
using Hsr.Model.Validators;

#endregion

namespace Hsr.Models
{
    [Validator(typeof (DetailInfoValidator))]
    [Table("USER_DETAIL_INFO", Schema = "MNCMS_APP")]
    public class UserDetailInfo : BaseModel
    {
        private string _birthdayText;

        [Key]
        [Column("DETAILAUID")]
        [ForeignKey("SummaryInfo")]
        public string UserAuid { get; set; }

        [Column("REAL_NAME")]
        [Display(Name ="真实姓名")]
        public string RealName { get; set; }

        [Column("COMP_ID")]
        [Display(Name = "公司名称")]
        public decimal? CompId { get; set; }

        [Column("COMP_NAME")]
        [Display(Name ="公司名称")]
        public string CompName { get; set; }

        [Column("DEPT_ID")]
        [Display(Name = "部门名称")]
        public decimal? DeptId { get; set; }

        [Column("DEPT_NAME")]
        [Display(Name = "部门名称")]
        public string DeptName { get; set; }

        [Column("SEX")]
        [Display(Name = "性别")]
        
        public decimal Sex { get; set; }

        [Column("TELPHONE")]
        [Display(Name = "座机号")]
        public string Telphone { get; set; }

        [Column("MOBILE")]
        [Display(Name ="手机号")]
        public string Mobile { get; set; }

        [Column("EMAIL")]
        [Display(Name ="邮箱")]
        public string Email { get; set; }

        [Column("ISENABLED")]
        [Display(Name = "是否有效")]
        [DefaultValue(1)]
        public decimal? Isenabled { get; set; }

        [Column("CREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        [Column("CREATE_USERID")]
        public string CreateUserid { get; set; }

        [Column("CREATE_BY")]
        public string CreateBy { get; set; }

        [Column("MODI_TIME")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? ModiTime { get; set; }

        [Column("MODI_USERID")]
        public string ModiUserid { get; set; }

        [Column("PHOTO")]
        public string Photo { get; set; }

        [Column("DESCRIPTION")]
        [Display(Name ="描述")]
        public string Description { get; set; }

        [Column("USERLEVEL")]
        public int UserLevel { get; set; }

        [Column("BIRTHDAY")]
        [Display(Name ="出生日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
        public DateTime? Birthday { get; set; }
 
        public virtual UserSummaryInfo SummaryInfo { get; set; }
    }
}