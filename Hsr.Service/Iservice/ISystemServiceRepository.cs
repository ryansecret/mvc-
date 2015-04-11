using Hsr.Model.Models;
using Hsr.Model.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsr.Service
{
    public interface ISystemServiceRepository
    {
        RoleVm GetRoleTreeItems();
        List<Sysrole> GetRoleByPId(int pid);
        TreeNode GetRoleTree();
        bool CreateRole(Sysrole model);
        bool EditRole(Sysrole model);
        bool DeleteRole(Sysrole model);
        Sysrole GetRoleByID(int id);
    }
}
