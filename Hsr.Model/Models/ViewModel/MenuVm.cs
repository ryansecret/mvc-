#region

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Hsr.Model.Validators;

#endregion

namespace Hsr.Model.Models.ViewModel
{
    [Validator(typeof (MenuValidator))]
    public class MenuVm
    {
        public decimal? Id { get; set; }

        [Display(Name = "名称")]
        public string Menuname { get; set; }

        public decimal? Pid { get; set; }


        public string PmenuName { get; set; }


        public decimal? Layer { get; set; }

        [Display(Name = "是否可用")]
        public bool Isenabled { get; set; }

        [Display(Name = "Action名称")]
        public string Action { get; set; }

        [Display(Name = "Controller名称")]
        public string Controller { get; set; }

        [Display(Name = "菜单图标")]
        public string Icon { get; set; }
        [Display(Name = "顺序")]
        public decimal? OrderNum { get; set; }

        public string ForbiddenAction { get; set; }
        public bool Seleted { get; set; }

        public List<MenuVm> Children { get; set; }

       

        public TreeNode ToNode()
        {
            return new TreeNode
            {
                NodeID = Id.HasValue ? (int) Id.Value : 0,
                NodeName = Menuname,
                ParentID = Pid.HasValue ? (int) Pid.Value : 0,
                Text = Menuname,
                IsSelected = Seleted
            };
        }

        public TreeJsonNode ToJsonNode()
        {
            return new TreeJsonNode()
            {
                id = Id.ToString(),
                text = Menuname,
                pid =Pid.ToString(),
                
                state = new StateType()
            };
        }
        
    }
}