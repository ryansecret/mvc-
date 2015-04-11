using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace Hsr.Core.Result
{
    public class SingleFileUpLoadAction:ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
          
            var response = context.HttpContext.Response;
            HttpRequest request = HttpContext.Current.Request;
            response.ContentType = "text/plain";
            try
            {
                byte[] buffer = new byte[1024 * 5];
                string tempDir = FileHelper.GetUploadPath();
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }
                
                foreach (string fileMark in request.Files)
                {
                    var file = request.Files[fileMark];
                    int read;
                    var stream = file.InputStream;
                    string fullName = string.Format("{0}\\{1}", tempDir,  file.FileName);
                    using (var fileStream = File.Open(fullName, FileMode.OpenOrCreate))
                    {
                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fileStream.Write(buffer, 0, read);
                        }
                    }
                }
               
                response.Write("上传成功！");
            }
            catch (Exception e)
            {
                response.Write(e.Message);
            }
        }
    }

    

    public class FileHelper
    {
        public FileHelper()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 获取上传目录
        /// </summary>
        /// <returns></returns>
        public static string GetUploadPath()
        {
            string path = HttpContext.Current.Server.MapPath("~/");
            string dirname = GetDirName();
            string uploadDir = path + "\\" + dirname;
            CreateDir(uploadDir);
            return uploadDir;
        }
        /// <summary>
        /// 获取临时目录
        /// </summary>
        /// <returns></returns>
        public static string GetTempPath()
        {
            string path = HttpContext.Current.Server.MapPath("~/");
            string dirname = GetTempDirName();
            string uploadDir = path + "\\" + dirname;
            CreateDir(uploadDir);
            return uploadDir;
        }
        private static string GetDirName()
        {
            return System.Configuration.ConfigurationManager.AppSettings["upLoadDir"];
        }
        private static string GetTempDirName()
        {
            return System.Configuration.ConfigurationManager.AppSettings["tempdir"];
        }
        public static void CreateDir(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }
    }
}
