using System.Threading.Tasks;
using Hsr.Model.Models.ViewModel;
using Hsr.Models;

namespace Hsr.Service.Service
{
    public interface IUserInfoService
    {
        Task<UserSummaryInfos> GetUsersAsync(int pid, UserSearParm param, UserSummaryInfos model);
        UserSummaryInfo GetUserById(string userId);
        bool Insert(UserSummaryInfo model);
        bool Edit(UserSummaryInfo model);
        bool Delete(string id);
        bool CheckUserName(string userName);
    }
}