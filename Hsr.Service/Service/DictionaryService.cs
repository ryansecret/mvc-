using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hsr.Core.Cache;
using Hsr.Core.Infrastructure;
using Hsr.Data.Interface;
using Hsr.Model.Models;
using Hsr.Models;
using Hsr.Service.Iservice;

namespace Hsr.Service.Service
{
    public class DictionaryService:IDictionaryService,IAsyncStartupTask
    {
        private ICacheManager _cacheManager;
        private IRepository<SysDictionaryInfo> _dictionaryRepository;
        const string DicKey = "DicKey"; 
        public DictionaryService(ICacheManager cacheManager,IRepository<SysDictionaryInfo> dicRepository)
        {
            _cacheManager = cacheManager;
            _dictionaryRepository = dicRepository;
        }
        public List<SysDictionaryInfo> GetDictionaryInfos(string groupName=null)
        {
            if (!_cacheManager.IsSet(DicKey))
            {
                var data= _dictionaryRepository.TableNoTracking.OrderBy(d=>d.Sort).ToList();
                _cacheManager.Set(DicKey,data,int.MaxValue);
            }
            if (string.IsNullOrWhiteSpace(groupName))
            {
                return _cacheManager.Get<List<SysDictionaryInfo>>(DicKey);
            }
            return _cacheManager.Get<List<SysDictionaryInfo>>(DicKey).Where(d=>d.GroupName==groupName).ToList();
        }

        public void Execute()
        {
            GetDictionaryInfos();
        }
    }
}
