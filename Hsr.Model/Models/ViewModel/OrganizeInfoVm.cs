#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using MVC.Controls.Paging;

#endregion

namespace Hsr.Model.Models.ViewModel
{
    public class OrganizeInfoVm
    {
      
        public decimal? OrgId { get; set; }

        [Required(ErrorMessage = "请输入公司名称")]
        [Display(Name = "公司名称")]
        public string OrgName { get; set; }

        [Display(Name = "所属机构")]
        [Required]
        public string OrgPName { get; set; }

        public decimal? OrgPid { get; set; }

        [Required(ErrorMessage = "请选择区")]
        [Display(Name ="所属地区")]
        public decimal? AreaId { get; set; }

        [Required(ErrorMessage = "请选择省")]
        [Display(Name = "所属省分")]
        public decimal? ProvinceId { get; set; }

        [Required(ErrorMessage = "请选择市")]
        [Display(Name = "所属城市")]
        public decimal? CityId { get; set; }
        
        public string Category { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUserid { get; set; }

        public string CreateBy { get; set; }

        [Required(ErrorMessage = "请输入手机号")]
        [RegularExpression(@"^1[3458][0-9]{9}$", ErrorMessage = "请输入正确的手机号")]
        [Display(Name ="手机号")]
        public string Phone { get; set; }

        [Display(Name = "传真")]
        public string Fax { get; set; }

        [Required(ErrorMessage = "请输入邮编")]
        [RegularExpression(@"^[1-9][0-9]{5}", ErrorMessage = "邮编格式不正确")]
        [Display(Name ="邮编")]
        public int? PostCode { get; set; }

        [Display(Name = "是否可用")]
        public bool Isenabled { get; set; }

        [Display(Name ="备注")]
        public string Description { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        public string Logo { get; set; }
        
        [Display(Name ="公司类型")]
        [Required]
        public decimal? OrgType { get; set; }
      

        public TreeJsonNode ToJsNode(bool setDisable=false)
        {
            return new TreeJsonNode
            {
                id = OrgId.ToString(),
                pid = OrgPid.ToString(),
                text = OrgName,
                icon = Category == "1" ? "/Images/pie.png" : "",
               
            };
        }
    }

    public class OrganizeInfoVms : BasePageableModel
    {
        public List<OrganizeInfoVm> Data { get; set; }
       
    }
}