using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MVC.Controls.Paging;

namespace MVC.Controls
{
    public static partial class HtmlExtension
    {
        public static Pager Pager(this HtmlHelper helper, IPageableModel pagination)
        {
             
            return new Pager(pagination, helper.ViewContext);
        }

        public static Pager Pager(this AjaxHelper helper, IPageableModel pagination)
        {
            return new Pager(pagination,helper.ViewContext);
        }

        public static Pager Pager(this HtmlHelper helper, string viewDataKey)
        {
            var dataSource = helper.ViewContext.ViewData.Eval(viewDataKey) as IPageableModel;

            if (dataSource == null)
            {
                throw new InvalidOperationException(string.Format("Item in ViewData with key '{0}' is not an IPagination.",
                                                                  viewDataKey));
            }

            return helper.Pager(dataSource);
        }

       
    }
}
