#region

using System.Collections.Generic;
using AutoMapper;
using Hsr.Model.Models.ViewModel;

#endregion

namespace Hsr.Model.Models
{
    public static class AutoMapperConverter
    {
        public static List<Menu> ToMenus(this List<MenuVm> menuVms)
        {
            return Mapper.Map<List<MenuVm>, List<Menu>>(menuVms);
        }

        public static List<MenuVm> ToMenuVms(this List<Menu> menus)
        {
            return Mapper.Map<List<Menu>, List<MenuVm>>(menus);
        }

        public static Menu ToMenu(this MenuVm menuVm)
        {
            return Mapper.Map<MenuVm, Menu>(menuVm);
        }

        public static MenuVm ToMenuVm(this Menu menu)
        {
            return Mapper.Map<Menu, MenuVm>(menu);
        }


        public static List<Sysrole> ToRoles(this List<RoleVm> roleVms)
        {
            return Mapper.Map<List<RoleVm>, List<Sysrole>>(roleVms);
        }

        public static List<RoleVm> ToRoleVms(this List<Sysrole> roles)
        {
            return Mapper.Map<List<Sysrole>, List<RoleVm>>(roles);
        }

        public static Sysrole ToRole(this RoleVm roleVm)
        {
            return Mapper.Map<RoleVm, Sysrole>(roleVm);
        }

        public static RoleVm ToRoleVm(this Sysrole role)
        {
            return Mapper.Map<Sysrole, RoleVm>(role);
        }


        public static List<OrganizeInfo> ToMenus(this List<OrganizeInfoVm> userSummaryVms)
        {
            return Mapper.Map<List<OrganizeInfoVm>, List<OrganizeInfo>>(userSummaryVms);
        }

        public static List<OrganizeInfoVm> ToMenuVms(this List<OrganizeInfo> menus)
        {
            return Mapper.Map<List<OrganizeInfo>, List<OrganizeInfoVm>>(menus);
        }

        public static OrganizeInfo ToMenu(this OrganizeInfoVm menuVm)
        {
            return Mapper.Map<OrganizeInfoVm, OrganizeInfo>(menuVm);
        }

        public static OrganizeInfoVm ToMenuVm(this OrganizeInfo menu)
        {
            return Mapper.Map<OrganizeInfo, OrganizeInfoVm>(menu);
        }


        public static List<OrganizeInfo> ToOrgs(this List<OrganizeInfoVm> orgVms)
        {
            return Mapper.Map<List<OrganizeInfoVm>, List<OrganizeInfo>>(orgVms);
        }

        public static List<OrganizeInfoVm> ToOrgVms(this List<OrganizeInfo> orgs)
        {
            return Mapper.Map<List<OrganizeInfo>, List<OrganizeInfoVm>>(orgs);
        }

        public static OrganizeInfo ToOrg(this OrganizeInfoVm orgVm)
        {
            return Mapper.Map<OrganizeInfoVm, OrganizeInfo>(orgVm);
        }

        public static OrganizeInfoVm ToOrgVm(this OrganizeInfo org)
        {
            return Mapper.Map<OrganizeInfo, OrganizeInfoVm>(org);
        }


        public static List<AreaInfo> ToAreas(this List<AreaVm> areaVms)
        {
            return Mapper.Map<List<AreaVm>, List<AreaInfo>>(areaVms);
        }

        public static List<AreaVm> ToAreaVms(this List<AreaInfo> areas)
        {
            return Mapper.Map<List<AreaInfo>, List<AreaVm>>(areas);
        }

        public static AreaInfo ToArea(this AreaVm areaVm)
        {
            return Mapper.Map<AreaVm, AreaInfo>(areaVm);
        }

        public static AreaVm ToAreaVm(this AreaInfo areaVm)
        {
            return Mapper.Map<AreaInfo, AreaVm>(areaVm);
        }
    }
}