using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Hsr.Core;
using Hsr.Core.Infrastructure;
using Hsr.Core.Result;
using Hsr.Service.FileOperate;
using Hsr.Service.Iservice;
using Hsr.Upload;
using Newtonsoft.Json;

namespace Hsr.Controllers
{
    public class CommonController:BaseController
    {
        private readonly IAreaService _areaService;
        private readonly IFileOperateService _fileOperateService;
        public CommonController(IAreaService areaService,IFileOperateService fileOperateService)
        {
            _areaService = areaService;
            _fileOperateService = fileOperateService;
        }
        public ActionResult GetCitys(decimal? id)
        {
            return new HsrJson() { Data = _areaService.GetCitys(id) };
            
        }
        
        public ActionResult GetAreas(decimal id)
        {
            return Json(_areaService.GetAreas(id),JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpLoad()
        {

            return EngineContext.Current.ContainerManager.ResolveUnregistered<UncFileUpLoadAction>();
        }
        [OutputCache(Duration = int.MaxValue)]
        public async Task<ActionResult> DownLoad(string fileName)
        {
            return await Task.Run(() => File(_fileOperateService.DownLoadFile(fileName), "image/png"));

        }
    }
}