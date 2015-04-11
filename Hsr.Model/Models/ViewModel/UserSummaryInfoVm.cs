using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hsr.Data;
using Hsr.Models;
using MVC.Controls.Paging;

namespace Hsr.Model.Models.ViewModel
{
    public class UserSummaryInfoVm 
    {
        public string Auid { get; set; }
        public int UserType { get; set; }
        public string UserName { get; set; }
        public string UserPw { get; set; }
        public string RealName { get; set; }
        //公司ID
        public decimal? CompId { get; set; }

        public string CompName { get; set; }
        //部门ID
        public decimal? DeptId { get; set; }

        public string DeptName { get; set; }
        //角色ID
        public decimal? RoleId { get; set; }
        
        public string RoleName { get; set; }
        public string Sex { get; set; }
        public string Mobile { get; set; }
        public string Telphone { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string Description { get; set; }
        public bool Isenabled { get; set; }
        public DateTime LoginTime { get; set; }
        public int LoginSum { get; set; }
        public bool Seleted { get; set; }
        public TreeNode ToNode()
        {
            return new TreeNode()
            {
                NodeID = this.DeptId.HasValue ? (int)DeptId.Value : 0,
                NodeName = DeptName,
                ParentID = CompId.HasValue ? (int)CompId.Value : 0,
                Text = DeptName,
                IsSelected = Seleted
            };
        }

        public List<UserSummaryInfoVm> Children { get; set; }

        
    }
}
