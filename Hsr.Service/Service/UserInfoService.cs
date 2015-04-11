using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Hsr.Data.Interface;
using Hsr.Model.Models;
using Hsr.Model.Models.ViewModel;
using Hsr.Models;
using Hsr.Service.Iservice;
using MVC.Controls;

namespace Hsr.Service.Service
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IRepository<UserSummaryInfo> _userSummaryInfoRepository;
        private readonly IRepository<OrganizeInfo> _organizeInfoRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
         
        private readonly IAreaService _areaService;

        public UserInfoService(IAreaService areaService, IRepository<UserSummaryInfo> userSumRepository, IRepository<OrganizeInfo> organizeInfoRepository, IRepository<UserRole> userRoleRepository)
        {
            _areaService = areaService;
            _userSummaryInfoRepository = userSumRepository;
            _organizeInfoRepository = organizeInfoRepository;
            _userRoleRepository = userRoleRepository;
            
        }

        public Task<UserSummaryInfos> GetUsersAsync(int pid, UserSearParm param, UserSummaryInfos model)
        {
            if (model.PageNumber <= 0)
            {
                model.PageNumber = 1;
            }
            if (model.PageSize <= 0)
            {
                model.PageSize = 15;
            }
            var task = Task.Factory.StartNew<UserSummaryInfos>(() =>
            {
                var data = _userSummaryInfoRepository.TableNoTracking;
                if (pid!=0)
                {
                    data = data.Where(d => d.Detail.CompId == pid);
                }
                if (param != null)
                {
                    if (!string.IsNullOrWhiteSpace(param.CompName))
                    {
                        data = data.Where(u => u.Detail.CompName.Contains(param.CompName));
                    }
                    if (!string.IsNullOrWhiteSpace(param.DeptName))
                    {
                        data = data.Where(u => u.Detail.DeptName.Contains(param.DeptName));
                    }
                    if (!string.IsNullOrWhiteSpace(param.RealName))
                    {
                        data = data.Where(u => u.Detail.RealName.Contains(param.RealName));
                    }
                    if (!string.IsNullOrWhiteSpace(param.RoleName))
                    {
                        data = data.Where(u => u.Role.Any(d => d.RoleName.Contains(param.RoleName)));
                    }
                }
 
                var pagedList =
                    new PagedList<UserSummaryInfo>(data.OrderBy(d => d.Detail.CreateTime), model.PageIndex,
                        model.PageSize);
                pagedList.ForEach(d =>
                {
                    var province = _areaService.GetProvince().FirstOrDefault(a => a.id == d.ProvinceId.ToString());
                    if (province != null)
                        d.ProvinceName = province.text;
                    var city = _areaService.GetCitys().FirstOrDefault(a => a.id == d.CityId.ToString());
                    if (city != null)
                        d.CityName = city.text;
                });

                model.LoadPagedList(pagedList);
                model.Data = pagedList;
                return model;
            });
            return task;
        }

        public UserSummaryInfo GetUserById(string userId)
        {
            UserSummaryInfo user = _userSummaryInfoRepository.GetById(userId);
            AreaVm province = _areaService.GetProvince().SingleOrDefault(d => d.id == user.ProvinceId.ToString());
            AreaVm city = _areaService.GetCitys(user.ProvinceId).SingleOrDefault(d => d.id == user.CityId.ToString());
            if (province != null) user.ProvinceName = province.text;
            if (city != null) user.CityName = city.text;
            user.RoleIds = string.Join(",", user.Role.Select(d => d.RoleId)); 
            return user;
        }

        /// <summary>
        ///  need to build cascade delete
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Insert(UserSummaryInfo model)
        {
            try
            {
                  OrganizeInfo organizeInfo = _organizeInfoRepository.TableNoTracking.SingleOrDefault(d => d.OrgId == model.Detail.CompId);
            if (organizeInfo != null)
            {
                model.ProvinceId = organizeInfo.ProvinceId;
                model.CityId = organizeInfo.CityId;
                model.AreaId = organizeInfo.AreaId;
            }
            model.Detail.Isenabled = 1;
            model.Detail.GenerateId();
                using (var transactionScope = new TransactionScope())
                {
                    model.LoginTime = DateTime.Now;
                    _userSummaryInfoRepository.Insert(model);
                    string[] ids = model.RoleIds.Split(',');
                    string[] names = model.RoleNames.Split(',');
                    for (int i = 0; i < ids.Count(); i++)
                    {
                        _userRoleRepository.Insert(new UserRole { RoleId = Convert.ToDecimal(ids[i]), RoleName = names[i], UserAuid = model.Auid });
                    }
                    transactionScope.Complete();
                }
            return true;
            }
            catch  
            {

                return false;
            }
          
        }

        public bool Edit(UserSummaryInfo model)
        {
            try
            {
                OrganizeInfo organizeInfo = _organizeInfoRepository.Table.SingleOrDefault(d => d.OrgId == model.Detail.CompId);
                if (organizeInfo != null)
                {
                    model.ProvinceId = organizeInfo.ProvinceId;
                    model.CityId = organizeInfo.CityId;
                    model.AreaId = organizeInfo.AreaId;
                }
                if (model.UserPw == null)
                {
                    model.UserPw = _userSummaryInfoRepository.GetById(model.Auid).UserPw;
                }
                model.Detail.GenerateId();
                model.Detail.ModiTime = DateTime.Now;
                model.Detail.Isenabled = 1;
                using (var transactionScope = new TransactionScope())
                {
 
                    _userSummaryInfoRepository.UpdateWithNav(model);

                    _userRoleRepository.ExecuteSqlCommand(string.Format("delete from USER_ROLE where USER_AUID='{0}' ", model.Auid));
                    string[] ids = model.RoleIds.Split(',');
                    string[] names = model.RoleNames.Split(',');
                    for (int i = 0; i < ids.Count(); i++)
                    {
                        _userRoleRepository.Insert(new UserRole { RoleId = Convert.ToDecimal(ids[i]), RoleName = names[i], UserAuid = model.Auid });
                    }
                    transactionScope.Complete();
                }
                return true;
            }
            catch
            {

                return false;
            }
        }

        public bool Delete(string id)
        {
            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    _userSummaryInfoRepository.Delete(id);
                  
                    _userRoleRepository.ExecuteSqlCommand(string.Format("delete from USER_ROLE where USER_AUID='{0}' ", id));
                    transactionScope.Complete();
                }
                return true;
            }
            catch
            {

                return false;
            }
        }

        public bool CheckUserName(string userName)
        {
            if (_userSummaryInfoRepository.TableNoTracking.Any(d=>d.UserName==userName))
            {
                return false;
            }
            return true;
        }
    }
}
