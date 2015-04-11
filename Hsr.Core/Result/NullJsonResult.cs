#region

using System;
using System.Web.Mvc;
using JsonSerializer = ServiceStack.Text.JsonSerializer;

#endregion

namespace Hsr.Core.Result
{
    public class NullJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");


            var response = context.HttpContext.Response;
            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";
            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            Data = null;

            //If you need special handling, you can call another form of SerializeObject below

            var serializedObject = JsonSerializer.SerializeToString(Data);
            response.Write(serializedObject);
        }
    }

    public class HsrJson : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");


            var response = context.HttpContext.Response;
            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";
            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            
            //If you need special handling, you can call another form of SerializeObject below

            var serializedObject = JsonSerializer.SerializeToString(Data);
            response.Write(serializedObject);
        }
    }
}