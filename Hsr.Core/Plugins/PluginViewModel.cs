using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsr.Core.Plugins
{
    public class PluginViewModel
    {
    
        public string Group { get; set; }
 
        public string FriendlyName { get; set; }
 
        public string SystemName { get; set; }
 
        public string Version { get; set; }

 
        public string Author { get; set; }

 
        public int DisplayOrder { get; set; }

 
        public string ConfigurationUrl { get; set; }

 
        public bool Installed { get; set; }

        public bool CanChangeEnabled { get; set; }
 
        public bool IsEnabled { get; set; }

        public string LogoUrl { get; set; }
    }
}
