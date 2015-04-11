using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Hsr.Core.Filters;
using Hsr.Core.Result;


namespace Hsr
{
    /// <summary>
    /// UpLoadSingle 的摘要说明
    /// </summary>
    /// 
    public class UpLoadSingle : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
          Process(context.Response,context.Request);
        }

        public void Process(HttpResponse response, HttpRequest request)
        {
            response.ContentType = "text/plain";
            try
            {
                var stream = request.InputStream;
                byte[] buffer = new byte[1024 * 5];
                string tempDir = FileHelper.GetUploadPath();
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }
                int read;
                string fullName = string.Format("{0}\\{1}", tempDir, request.Form["fileToUpload"]);
                using (var fileStream = File.Open(fullName, FileMode.OpenOrCreate))
                {
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fileStream.Write(buffer, 0, read);
                    }
                }
                response.Write("上传成功！");
            }
            catch (Exception e)
            {
                response.Write(e.Message);
            }
           
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

   
}