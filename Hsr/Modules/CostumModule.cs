using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using System.Web.Routing;
using System.Web.SessionState;
using Autofac.Core;
using Microsoft.Web.Infrastructure.DynamicValidationHelper;

namespace Hsr.Modules
{
    /// <summary>
    ///  可建立权限验证模块
    /// </summary>
    public class CostumModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += context_AuthenticateRequest;
            context.AuthorizeRequest += context_AuthorizeRequest;
            context.BeginRequest += (s, e) =>
            {
                
                HttpApplication app = s as HttpApplication;
                MvcHandler mvcHandler = new MvcHandler(context.Request.RequestContext);
                IController controller;
                IControllerFactory factory;
                //HttpContextBase httpContextBase = new HttpContextWrapper(context.Context);
                //if (context.Context.Request.RequestContext.RouteData.Values.Count>1)
                //{
                //    mvcHandler.ProcessRequestInit(httpContextBase, out controller, out factory);
                //}
                
                
                //if (app!=null)
                //{ 
                   
                //    app.Response.Write("this is ryan test");
                   
                //}
            };
        }

        void context_AuthorizeRequest(object sender, EventArgs e)
        {
            
        }

        void context_AuthenticateRequest(object sender, EventArgs e)
        {
            
        }

        public void Dispose()
        {
             
        }
    }

    public class MvcHandler 
    {
        private static readonly object _processRequestTag = new object();

        internal static readonly string MvcVersion = GetMvcVersionString();
        public static readonly string MvcVersionHeaderName = "X-AspNetMvc-Version";
        private ControllerBuilder _controllerBuilder;

        public MvcHandler(RequestContext requestContext)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }

            RequestContext = requestContext;
        }

        internal ControllerBuilder ControllerBuilder
        {
            get
            {
                if (_controllerBuilder == null)
                {
                    _controllerBuilder = ControllerBuilder.Current;
                }
                return _controllerBuilder;
            }
            set { _controllerBuilder = value; }
        }

        public static bool DisableMvcResponseHeader { get; set; }

        protected virtual bool IsReusable
        {
            get { return false; }
        }

        public RequestContext RequestContext { get; private set; }

        protected internal virtual void AddVersionHeader(HttpContextBase httpContext)
        {
            if (!DisableMvcResponseHeader)
            {
                httpContext.Response.AppendHeader(MvcVersionHeaderName, MvcVersion);
            }
        }

        //protected virtual IAsyncResult BeginProcessRequest(HttpContext httpContext, AsyncCallback callback, object state)
        //{
        //    HttpContextBase httpContextBase = new HttpContextWrapper(httpContext);
        //    return BeginProcessRequest(httpContextBase, callback, state);
        //}

        

       

        private static string GetMvcVersionString()
        {
            // DevDiv 216459:
            // This code originally used Assembly.GetName(), but that requires FileIOPermission, which isn't granted in
            // medium trust. However, Assembly.FullName *is* accessible in medium trust.
            return new AssemblyName(typeof(MvcHandler).Assembly.FullName).Version.ToString(2);
        }

        protected virtual void ProcessRequest(HttpContext httpContext)
        {
            HttpContextBase httpContextBase = new HttpContextWrapper(httpContext);
            ProcessRequest(httpContextBase);
        }

        public   virtual void ProcessRequest(HttpContextBase httpContext)
        {
            IController controller;
            IControllerFactory factory;
            ProcessRequestInit(httpContext, out controller, out factory);

            try
            {
                controller.Execute(RequestContext);
            }
            finally
            {
                factory.ReleaseController(controller);
            }
        }

        public void ProcessRequestInit(HttpContextBase httpContext, out IController controller, out IControllerFactory factory)
        {
            // If request validation has already been enabled, make it lazy. This allows attributes like [HttpPost] (which looks
            // at Request.Form) to work correctly without triggering full validation.
            // Tolerate null HttpContext for testing.
            HttpContext currentContext = HttpContext.Current;
            if (currentContext != null)
            {
                bool? isRequestValidationEnabled = ValidationUtility.IsValidationEnabled(currentContext);
                if (isRequestValidationEnabled == true)
                {
                    ValidationUtility.EnableDynamicValidation(currentContext);
                }
            }

            AddVersionHeader(httpContext);
            RemoveOptionalRoutingParameters();
            try
            {
                string controllerName = httpContext.Request.RequestContext.RouteData.GetRequiredString("controller");
               
                // Instantiate the controller and call Execute
                factory = ControllerBuilder.GetControllerFactory();
                controller = factory.CreateController(RequestContext, controllerName);
                if (controller == null)
                {
                    throw new InvalidOperationException(
                        String.Format(
                            CultureInfo.CurrentCulture,
                            "",
                            factory.GetType(),
                            controllerName));
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            // Get the controller type
          
        }

        private void RemoveOptionalRoutingParameters()
        {
            RouteValueDictionary rvd = RequestContext.RouteData.Values;

            // Ensure delegate is stateless
            
        }

        

        #region IHttpAsyncHandler Members

        //IAsyncResult IHttpAsyncHandler.BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        //{
        //    return BeginProcessRequest(context, cb, extraData);
        //}

        //void IHttpAsyncHandler.EndProcessRequest(IAsyncResult result)
        //{
        //    EndProcessRequest(result);
        //}

        #endregion

        // Keep as value type to avoid allocating
        private struct ProcessRequestState
        {
            internal IAsyncController AsyncController;
            internal IControllerFactory Factory;
            internal RequestContext RequestContext;

            internal void ReleaseController()
            {
                Factory.ReleaseController(AsyncController);
            }
        }
    }
}