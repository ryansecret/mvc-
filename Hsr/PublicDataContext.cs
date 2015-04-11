using System.Data.Entity;
using Hsr.Data;
using Hsr.Model.Models;
using Hsr.Models;

namespace Hsr
{
    public class PublicDataContext : BaseObjectContext
    {
        public IDbSet<ControllerFilterData> Msgs { get; set; }
        public IDbSet<Ryan> Ryans { get; set; }
     
        public IDbSet<Sysrole> Roles { get; set; }

        public IDbSet<Menu> Menus { get; set; }
        public IDbSet<OrganizeInfo> OrganizeInfos { get; set; }
        public IDbSet<UserSummaryInfo> UserSummaryInfos { get; set; }
        public IDbSet<UserDetailInfo> UserDetailInfos { get; set; }
        public IDbSet<UserRole> UserRoles { get; set; }
        public IDbSet<AreaInfo> Areainfos { get; set; }
        public IDbSet<Authority> Authoritys { get; set; }
        public IDbSet<Datamapping> Datamappings { get; set; }
        public IDbSet<DatamappingColumn> DatamappingColumns { get; set; }

        public IDbSet<SysDictionaryInfo> DictionaryInfos { get; set; } 

        public IDbSet<DataRule> Rules { get; set; } 

        public IDbSet<CommTestPlan> TestPlans { get; set; } 
    }

     
}