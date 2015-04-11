using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hsr.Core.Cache;
using Hsr.Core.Infrastructure;
using Hsr.Data.Interface;
using Hsr.Model.Models;
using Hsr.Model.Models.ViewModel;
using Hsr.Service.Iservice;

namespace Hsr.Service.Service
{
    public class AreaService : IAreaService, IAsyncStartupTask
    {
        private readonly IRepository<AreaInfo> _areaDal;
        private readonly ICacheManager _cacheManager;
        public AreaService(IRepository<AreaInfo> areaDal,ICacheManager cacheManager)
        {
            _areaDal = areaDal;
            _cacheManager = cacheManager;
        }
       

        public List<AreaVm> GetProvince()
        {
            if (!_cacheManager.IsSet(Global.Province))
            {
                var province = GetAllAreas().GroupBy(d => new { proname = d.ProvinceName, proid = d.ProvinceId }).ToList().Select(d => new AreaVm() { text = d.Key.proname, id = d.Key.proid.ToString() }).ToList();

                _cacheManager.Set(Global.Province, province, Int32.MaxValue);
            }
           
            return _cacheManager.Get<List<AreaVm>>(Global.Province);
        }

        public List<AreaVm> GetCitys(decimal? proinceId=null)
        {
            var areas = GetAllAreas();
            if (proinceId.HasValue)
            {
                areas =
                    areas.Where(d => d.ProvinceId == proinceId.Value);
            }
            List<AreaVm> citys;
            var key = proinceId.HasValue ? Global.Citys + proinceId : Global.Citys;
            if (!_cacheManager.IsSet(key))
            {
                citys = areas.GroupBy(d => new { AreaId = d.CityId, AreaName = d.CityName,Pid=d.ProvinceId })
                       .ToList()
                       .Select(d => new AreaVm() { text = d.Key.AreaName, id = d.Key.AreaId.ToString(), pid = d.Key.Pid.ToString()}).ToList();
                _cacheManager.Set(key, citys, int.MaxValue);
            }
            return _cacheManager.Get<List<AreaVm>>(key);
        }

        public List<AreaVm> GetAreas(decimal? cityId=null)
        {
            var areas = GetAllAreas();
            if (cityId.HasValue)
            {
                areas =
                    areas.Where(d => d.CityId == cityId.Value);
            }
            List<AreaVm> citys;
            var key = cityId.HasValue ? Global.Areas + cityId : Global.Areas;
            if (!_cacheManager.IsSet(key))
            {
                citys = areas.GroupBy(d => new { AreaId = d.AreaId, AreaName = d.AreaName,Pid=d.CityId })
                       .ToList()
                       .Select(d => new AreaVm() { text = d.Key.AreaName, id = d.Key.AreaId.ToString(),pid=d.Key.Pid.ToString() }).ToList();
                _cacheManager.Set(key, citys, int.MaxValue);
            }
            return _cacheManager.Get<List<AreaVm>>(key);
        }

        public IEnumerable<AreaInfo> GetAllAreas()
        {
            const string key = "AllAreas";
            if (!_cacheManager.IsSet(key))
            {
                _cacheManager.Set(key,_areaDal.TableNoTracking.ToList(),int.MaxValue);
            }
            return _cacheManager.Get<List<AreaInfo>>(key);
        }

        public void Execute()
        {
            GetAllAreas();
            GetProvince();
            GetCitys();
            GetAreas();
        }
    }
}
