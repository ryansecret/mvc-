using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hsr.Models;

namespace Hsr.Service.Iservice
{
    public interface IDictionaryService
    {
        List<SysDictionaryInfo> GetDictionaryInfos(string groupName);
    }
}
