using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hsr.Core.Result;
using Hsr.Service.FileOperate;

namespace Hsr.Upload
{
   
    public class UncFileUpLoadAction : ActionResult
    { 
        private IFileOperateService _fileOperateService;
        public UncFileUpLoadAction(IFileOperateService fileOperateService)
        {
            _fileOperateService = fileOperateService;
        }
        public override void ExecuteResult(ControllerContext context)
        {

            var response = context.HttpContext.Response;
            HttpRequest request = HttpContext.Current.Request;
            response.ContentType = "text/plain";
            try
            {

                var path=  _fileOperateService.UpLoadFiles(request.Files);
                response.Write(path);
            }
            catch (Exception e)
            {
                response.Write("0");
            }
        }
    }

 
}