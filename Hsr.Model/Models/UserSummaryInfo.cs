#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Hsr.Data;
using Hsr.Data.CustomAttribute;
using Hsr.Model.Models;
using Hsr.Model.Validators;
using MVC.Controls.Paging;

#endregion

namespace Hsr.Models
{
    [Validator(typeof(UserSummaryInfoValidator))]
    [Table("USER_SUMMARY_INFO", Schema = "MNCMS_APP")]
    public class UserSummaryInfo : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("Detail")]
        [KeyGenerate(KeyKind = KeyKind.Guid)]
        [Column("AUID")]
        public string Auid { get; set; }

        [Column("LAST_ACTIVE_TIME")]
        public DateTime? LastActiveTime { get; set; }

        [Column("USER_GUID")]
        public string UserGuid { get; set; }

        [Column("PASSWORD_TIME")]
        public DateTime? PasswordTime { get; set; }

        [Column("LOGIN_IP")]
        public string LoginIp { get; set; }

        [Column("LOGIN_TIME")]
        public DateTime? LoginTime { get; set; }

        [Column("LOGIN_SUM")]
        public int? LoginSum { get; set; }

        [Column("ISLINE")]
        public decimal? Isline { get; set; }

        [Column("AREA_ID")]
        public decimal? AreaId { get; set; }

        [NotMapped]
        public string AreaName { get; set; }

        [Column("CITY_ID")]
        public decimal? CityId { get; set; }

        [NotMapped]
        public string CityName { get; set; }

        [Column("PROVINCE_ID")]
        public decimal? ProvinceId { get; set; }

        [NotMapped]
        public string ProvinceName { get; set; }

        [Column("USER_PW")]
        [DataType(DataType.Password)]
        [Display(Name = "用户密码")]
        public string UserPw { get; set; }

        [Column("USER_NAME")]
        [Display(Name = "用户名")]
        [Remote("CheckUserName", "UserSummaryInfo",ErrorMessage ="用户名已被占用")]
        public string UserName { get; set; }

        [Column("USER_TYPE")]
        public int UserType { get; set; }

        [NotMapped]
        [Display(Name ="角色")]
        public string RoleIds { get; set; }

        [NotMapped]
        public string RoleNames { get; set; }
        [NotMapped]
        [Display(Name = "确认密码")]
        [DataType(DataType.Password)]
        public string UserPwCompare { get; set; }

        public virtual UserDetailInfo Detail { get; set; }

        public virtual ICollection<UserRole> Role { get; set; }
    }

    public class UserSummaryInfos : BasePageableModel
    {
        public List<UserSummaryInfo> Data { get; set; }
         
    }
}