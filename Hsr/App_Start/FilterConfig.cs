using System.Web;
using System.Web.Mvc;
using Hsr.Core.Filters;
using HandleErrorAttribute = Hsr.Core.Filters.HandleErrorAttribute;

namespace Hsr
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeFilter());
            
            //filters.Add(new RemoveDuplicateContentAttribute());
        }
    }
}