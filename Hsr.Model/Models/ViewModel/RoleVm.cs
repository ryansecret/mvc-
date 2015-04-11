#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MVC.Controls.Paging;

#endregion

namespace Hsr.Model.Models.ViewModel
{
    public class RoleVm
    {
        public decimal? RoleId { get; set; }
        [Required(ErrorMessage = "请输入角色名")]
        [Display(Name ="角色名称")]
        public string RoleName { get; set; }
        [Display(Name ="角色分类")]
        public decimal? Category { get; set; }

        [Display(Name = "是否可用")]
        public bool Isenabled { get; set; }
        [Display(Name ="描述")]
        public string Description { get; set; }

        public DateTime CreateTime { get; set; }

        public decimal? RolePid { get; set; }

        public string CreateUserid { get; set; }
　
        public string CreateBy { get; set; }

        public List<Sysrole> returnlist { get; set; }

        public TreeNode node { get; set; }

        public TreeNode ToNode()
        {
            return new TreeNode
            {
                NodeID = RoleId.HasValue ? (int) RoleId.Value : 0,
                NodeName = RoleName,
                ParentID = RolePid.HasValue ? (int) RolePid.Value : 0,
                Text = RoleName
                 
            };
        }
    }

    public class RoleVms : BasePageableModel
    {
        public List<RoleVm> Data { get; set; }
        　
    }
}