using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Hsr.Model.Models;
using Hsr.Model.Models.ViewModel;

namespace Hsr.Service.Iservice
{
    public interface IAreaService
    {
        List<AreaVm> GetProvince();
        List<AreaVm> GetCitys(decimal? proinceId=null);

        List<AreaVm> GetAreas(decimal? cityId=null);

        IEnumerable<AreaInfo> GetAllAreas();
    }
}
