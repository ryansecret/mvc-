using Hsr.Model.Models;
using Hsr.Model.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hsr.Models;

namespace Hsr.Service
{
    public interface IOrganizeInfoService
    {
        OrganizeInfoVm GetOrganizeInfoTreeItems();
        List<OrganizeInfo> GetOrganizeInfoByPId(int pid);
        TreeNode GetOrganizeInfoTree(int pid);
        bool CreateOrganizeInfo(OrganizeInfo model);
        bool EditOrganizeInfo(OrganizeInfo model);
        bool DeleteOrganizeInfo(OrganizeInfo model);
        OrganizeInfo GetOrganizeInfoByID(int id);
    }
}
