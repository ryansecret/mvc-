using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using AutoMapper;
using Hsr.Core;
using Hsr.Core.Plugins;

namespace Hsr.Controllers
{
    public class PluginController : HsrBaseController
    {
        private readonly IPluginFinder _pluginFinder;
        private readonly IWebHelper _webHelper;
        public PluginController(IPluginFinder pluginFinder,IWebHelper webHelper)
        {
            _pluginFinder = pluginFinder;
            _webHelper = webHelper;
        }
        public ActionResult Index()
        {
            var pluginDescriptors = _pluginFinder.GetPluginDescriptors(false).ToList();
            var models=  pluginDescriptors.Select(d => ToPluginViewModel(d));
            return View(models);
        }

        private PluginViewModel ToPluginViewModel(PluginDescriptor pluginDescriptor)
        {
            var pluginModel = Mapper.Map<PluginDescriptor, PluginViewModel>(pluginDescriptor);
            pluginModel.LogoUrl = pluginDescriptor.GetLogoUrl(_webHelper);
            return pluginModel;
        }

        public ActionResult Install(string systemName)
        {
                
                var pluginDescriptor = _pluginFinder.GetPluginDescriptors(false)
                    .FirstOrDefault(x => x.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
                if (pluginDescriptor == null)
                    //No plugin found with the specified id
                    return RedirectToAction("Index");

                //check whether plugin is not installed
                if (pluginDescriptor.Installed)
                    return RedirectToAction("Index");
               
               // PluginManager.LoadAssembly(pluginDescriptor);
                //install plugin
                PluginManager.MarkPluginAsInstalled(pluginDescriptor.SystemName);
                pluginDescriptor.Installed = true;
                //restart application
                _webHelper.RestartAppDomain();
                return RedirectToAction("Index");
        }
        public ActionResult Uninstall(string systemName)
        {
         
                var pluginDescriptor = _pluginFinder.GetPluginDescriptors(false)
                    .FirstOrDefault(x => x.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
                if (pluginDescriptor == null)
                    //No plugin found with the specified id
                    return RedirectToAction("Index");

                //check whether plugin is installed
                if (!pluginDescriptor.Installed)
                    return RedirectToAction("Index");

                PluginManager.MarkPluginAsUninstalled(pluginDescriptor.SystemName);
                //restart application
                pluginDescriptor.Installed = false;
                 _webHelper.RestartAppDomain();
                
                return RedirectToAction("Index");
        }
    }
}
