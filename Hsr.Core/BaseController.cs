#region

using System.Text;
using System.Web.Mvc;
using Hsr.Core.Result;
using MVC.Controls.Recaptcha;
#endregion
namespace Hsr.Core
{
    
    public class BaseController : Controller
    {
        protected virtual ActionResult InvokeHttp404()
        {
            // Call target Controller and pass the routeData.
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return new EmptyResult();
        }

        public ActionResult GetValidateCode(int length)
        {
            var vCode = new ValidateCode();
            var code = vCode.CreateValidateCode(length);
            var bytes = vCode.CreateValidateGraphic(code);
            Session["ValidateCode"] = code;
            return File(bytes, @"image/gif");
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
    }
}