using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Hsr.Core.Infrastructure;
using Hsr.Core.Plugins;
using Hsr.Model.Models;
using Hsr.Model.Models.ViewModel;
using Hsr.Models;

namespace Hsr.App_Start
{
    public class AutoMapperStartupTask : IStartupTask
    {
        public int Order { get { return 0; }}
        public void Execute()
        {
            Mapper.CreateMap<int, decimal>().ConvertUsing(Convert.ToDecimal);
            Mapper.CreateMap<decimal, int>().ConvertUsing(Convert.ToInt32);
            Mapper.CreateMap<ControllerFilterDataVm, ControllerFilterData>();
            Mapper.CreateMap<ControllerFilterData, ControllerFilterDataVm>();
            Mapper.CreateMap<Menu, MenuVm>().ForMember(d=>d.Isenabled,s=>s.MapFrom(d=>d.Isenabled==1));
            Mapper.CreateMap<MenuVm, Menu>().ForMember(d=>d.Isenabled,s=>s.MapFrom(d=>d.Isenabled?1:0));
            Mapper.CreateMap<Sysrole, RoleVm>().ForMember(d => d.Isenabled, s => s.MapFrom(d => d.Isenabled == 1));
            Mapper.CreateMap<RoleVm, Sysrole>()
                .ForMember(d => d.Isenabled, s => s.MapFrom(d => d.Isenabled ? 1m : 0m)).ForMember(d => d.RolePid, s => s.MapFrom(d => d.RolePid.HasValue?d.RolePid.Value:0));
            Mapper.CreateMap<OrganizeInfo, OrganizeInfoVm>().ForMember(d => d.Isenabled, s => s.MapFrom(d => d.Isenabled == 1));
            Mapper.CreateMap<OrganizeInfoVm, OrganizeInfo>().ForMember(d => d.Isenabled, s => s.MapFrom(d => d.Isenabled ? 1 : 0));
            Mapper.CreateMap<AreaInfo, AreaVm>();
            Mapper.CreateMap<AreaVm, AreaInfo>();

            Mapper.CreateMap<PluginDescriptor, PluginViewModel>();
        }
    }
}