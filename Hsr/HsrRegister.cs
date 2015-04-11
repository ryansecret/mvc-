using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Hsr.Core;
using Hsr.Core.Cache;
using Hsr.Core.Cache.Memcache;
using Hsr.Core.Infrastructure;
using Hsr.Core.Infrastructure.DependencyManagement;
using Hsr.Core.Log;
using Hsr.Core.Plugins;
using Hsr.Data;
using Hsr.Data.Interface;
using Hsr.Service;
using Hsr.Service.FileOperate;
using Hsr.Service.Iservice;
using Hsr.Service.Service;
using Nop.Web.Framework.Mvc.Routes;

namespace Hsr
{
    public class HsrRegister : IDependencyRegister
    {
        public int Order { get { return 0; }}
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
              builder.Register(c => new HttpContextWrapper(HttpContext.Current) as HttpContextBase)
                .As<HttpContextBase>()
                .InstancePerHttpRequest();
            builder.RegisterInstance(new Log4Manager()).As<Log4Manager>().SingleInstance();
              builder.RegisterType<PluginFinder>().As<IPluginFinder>().InstancePerHttpRequest();
            builder.Register<IDbContext>(c=>new PublicDataContext()).InstancePerHttpRequest();
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerHttpRequest();
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());
            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();
            builder.RegisterType<MemcachedClientCache>().As<IMemcachedClient>().SingleInstance();
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().SingleInstance();
            //builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("nop_cache_per_request").InstancePerHttpRequest();
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerHttpRequest();

            #region serviceRegister
          
            builder.RegisterType<OrganizeInfoService>().AsImplementedInterfaces();
            builder.RegisterType<AreaService>().AsImplementedInterfaces();
            builder.RegisterType<UserInfoService>().AsImplementedInterfaces();
            builder.RegisterType<FileOperateService>().AsImplementedInterfaces();
            builder.RegisterType<DictionaryService>().AsImplementedInterfaces();

            #endregion
        }
 
    }
}