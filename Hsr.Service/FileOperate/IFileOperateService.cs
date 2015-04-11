using System.Web;

namespace Hsr.Service.FileOperate
{
    public interface IFileOperateService
    {
        string UpLoadFiles(HttpFileCollection fileCollection, int province = 15, string appendDir = "Images");
        byte[] DownLoadFile(string filePath,int province=15);
    }
}