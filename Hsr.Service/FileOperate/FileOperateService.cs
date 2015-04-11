using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Hsr.Core.Infrastructure;
using Hsr.Core.Log;
using Hsr.Data.Interface;
using Hsr.Models;

namespace Hsr.Service.FileOperate
{
    public class FileOperateService : IFileOperateService
    {
        private readonly IRepository<DataRule> _dataRule;
        private readonly Log4Manager _logger;
        public FileOperateService(IRepository<DataRule> dataRule)
        {
            _dataRule = dataRule;
            _logger = EngineContext.Current.Resolve<Log4Manager>();
        }

        public string UpLoadFiles(HttpFileCollection fileCollection, int province = 15, string appendDir = "Images")
        {
            string uploadedPath=string.Empty;
            var dataNode = _dataRule.TableNoTracking.FirstOrDefault(d => d.ProvinceId == province);
            if (dataNode!=null&&dataNode.NodeInfo!=null)
            {
                using (var unc = new UncAccessWithCredentials())
                { 
                    var config = dataNode.NodeInfo;
                    string serverPath = string.Format(@"\\{0}", config.NodeIp);

                    var basePath = GetFilePath(serverPath, config.CifsDir,appendDir);
                    uploadedPath = Path.Combine(config.CifsDir, appendDir);
                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }

                    if (unc.NetUseWithCredentials(basePath, config.CifsAccout, "", config.CifsPasswd))
                    {
                        foreach (string fileMark in fileCollection)
                        {
                            byte[] buffer = new byte[1024 * 5];
                            var file = fileCollection[fileMark];
                            int read;
                            var stream = file.InputStream;
                            string fullName = string.Format("{0}\\{1}", basePath, file.FileName);
                            using (var fileStream = File.Open(fullName, FileMode.OpenOrCreate))
                            {
                                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    fileStream.Write(buffer, 0, read);
                                }
                            }
                        }
                    }
                    else
                    {
                        _logger.Info(string.Format(" 认证文件失败 ,原因：{3},地址：{0} 用户名：{1} 密码：{2}", basePath, config.CifsAccout, config.CifsPasswd, unc.LastError));
                        return null;
                    }
                }
            }
            else
            {
              
                _logger.Info("数据库缺少相关配置！");
                return null;
            }
            return uploadedPath;
        }

        public byte[] DownLoadFile(string filePath,int province=15)
        {
            var dataNode = _dataRule.TableNoTracking.FirstOrDefault(d => d.ProvinceId == province);
            if (dataNode!=null&&dataNode.NodeInfo!=null)
            {
                using (var unc = new UncAccessWithCredentials())
                {
                    var config = dataNode.NodeInfo;
                    string serverPath = string.Format(@"\\{0}", config.NodeIp);
                     
                    filePath = Path.Combine(serverPath, filePath);
                    var basePath = Path.Combine(serverPath, config.CifsDir);
                    if (unc.NetUseWithCredentials(basePath, config.CifsAccout, "", config.CifsPasswd))
                    {
                        if (File.Exists(filePath))
                        {
                          return   File.ReadAllBytes(filePath);
                        }
                        _logger.Info(string.Format(" 认证文件失败 ,原因：{3},地址：{0} 用户名：{1} 密码：{2}", basePath, config.CifsAccout, config.CifsPasswd, unc.LastError));
                        return null;
                    }
                    else
                    {
                        _logger.Info(string.Format(" 认证文件失败 ,原因：{3},地址：{0} 用户名：{1} 密码：{2}", basePath, config.CifsAccout, config.CifsPasswd, unc.LastError));
                        return null;
                    }
                }
            }
            else
            {
                _logger.Info("数据库缺少相关配置！");
                return null;
            }
        }

        private string GetFilePath(string rawPath,string part,string appendDir)
        {
            var list = part.Split(new[] { '/' }).ToList();

            string filePath = rawPath;
            
            foreach (var path in list)
            {
                filePath = Path.Combine(filePath, path);
            }
            filePath = Path.Combine(filePath, appendDir);
            return filePath;
        }
    }
}
