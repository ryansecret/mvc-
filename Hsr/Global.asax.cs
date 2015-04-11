using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Mvc;
using FluentValidation.Resources;
using FluentValidation.Validators;
using Hsr.App_Start;
using Hsr.Core;
using Hsr.Core.Infrastructure;
using Hsr.Core.Infrastructure.DependencyManagement;
using Hsr.Core.Log;
using Hsr.FluentCnResource;
using Hsr.Model.CustomValidators;
using Hsr.Model.Validators;
using Hsr.Models;
using Hsr.Modules;
using Microsoft.Ajax.Utilities;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using StackExchange.Profiling;
using EqualValidator = Hsr.Model.CustomValidators.EqualValidator;

namespace Hsr
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        { 
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("zh-CN");
            var dependencyResolver = new HsrDependencyResolver();
            DependencyResolver.SetResolver(dependencyResolver);
            EngineContext.Initialize(false);
            AreaRegistration.RegisterAllAreas();
  
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
          //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
          //  BundleTable.EnableOptimizations = true;
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
 
            ConfigureFluentValidation();
            this.AuthenticateRequest += MvcApplication_AuthenticateRequest;
            this.AuthorizeRequest += MvcApplication_AuthorizeRequest;
            //DisplayModeProvider.Instance.Modes.Clear();
            //DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("mobile")
            //{
            //    ContextCondition = (context => { return true; })
            //});
            ;
        }

        void MvcApplication_AuthorizeRequest(object sender, EventArgs e)
        {
            Console.WriteLine("");
        }

        void MvcApplication_AuthenticateRequest(object sender, EventArgs e)
        {
            Console.WriteLine("");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (CanPerformProfilingAction())
            {
                MiniProfiler.Start();
            }
           
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (CanPerformProfilingAction())
            {
                MiniProfiler.Stop();
            }
          
            //dispose registered resources
            //we do not register AutofacRequestLifetimeHttpModule as IHttpModule 
            //because it disposes resources before this Application_EndRequest method is called
            //and in this case the code in Application_EndRequest of Global.asax will throw an exception
            AutofacRequestLifetimeHttpModule.ContextEndRequest(sender, e);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
 
            LogException(exception);
        }

        protected void LogException(Exception exc)
        {
            if (exc == null)
                return;
            var httpException = exc as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                var logger = EngineContext.Current.ContainerManager.ResolveUnregistered<Log4Manager>();

                logger.Error(exc.Message, exc);
                return;
            }
            
            try
            {
                var logger = EngineContext.Current.Resolve<Log4Manager>();
             
                logger.Error(exc.Message, exc);
            }
            catch (Exception)
            {
               
            }
        }
        protected bool CanPerformProfilingAction()
        {
            return false;
            //will not run in medium trust
            if (CommonHelper.GetTrustLevel() < AspNetHostingPermissionLevel.High)
                return false;

            return true;
        }

        protected void ConfigureFluentValidation()
        {

            ValidatorOptions.ResourceProviderType = typeof(ValidationResource);

            
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure; // ValidatorOptions.CascadeMode 默认值为：CascadeMode.Continue
            FluentValidationModelValidatorProvider.Configure();
        }
    }
     
}