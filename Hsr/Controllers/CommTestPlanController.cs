using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Hsr.Core;
using Hsr.Data.Interface;
using Hsr.Model.Models;
using Hsr.Model.Models.ViewModel;
using Hsr.Models;
using System.Data.Entity;
using Hsr.Models;
using Hsr.Service;
using Hsr.Service.Iservice;
using MVC.Controls;
using MVC.Controls.Paging;
using WebGrease.Css.Extensions;

namespace Hsr.Controllers
{   
    public class CommTestPlanController : HsrBaseController
    {
		private readonly IRepository<CommTestPlan> _commtestplanRepository;
        private readonly IDictionaryService _dictionaryService;
        private readonly IAreaService _areaService;
        public CommTestPlanController(IRepository<CommTestPlan> commtestplanRepository,IDictionaryService dictionaryService,IAreaService areaService)
        {
			this._commtestplanRepository = commtestplanRepository;
            _dictionaryService = dictionaryService;
            _areaService = areaService;
        }

        //
        // GET: /CommTestPlan/

        public async Task<ViewResult> Index()
        {
            var data = await GetUsersAsync(CurrentUser.ProvinceId, null, new ComTestPlanPage());
            return View(data);
        }

        public async Task<ActionResult> GetList(decimal? areaId, TestPlanParam param,ComTestPlanPage model)
        {
            var data = await GetUsersAsync(areaId, param, model);
           
            return  PartialView("_PlanList", data);
        }


        private Task<ComTestPlanPage> GetUsersAsync(decimal? areaId, TestPlanParam param,ComTestPlanPage model)
        {
            if (model.PageNumber <= 0)
            {
                model.PageNumber = 1;
            }
            if (model.PageSize <= 0)
            {
                model.PageSize = 15;
            }
            var task = Task.Factory.StartNew<ComTestPlanPage>(() =>
            {
                var data = _commtestplanRepository.TableNoTracking;
                if (areaId.HasValue&&areaId != 0)
                {
                    if (areaId>0&&areaId<100)
                    {
                        data = data.Where(d => d.ProvinceId == areaId);
                    }
                    else
                    {
                        if (areaId>10000)
                        {
                            data = data.Where(d => d.AreaId == areaId);
                        }
                        else
                        {
                            data = data.Where(d => d.CityId == areaId);
                        }
                    }
                    
                }
                if (param != null)
                {  
                    if (param.BeginTime.HasValue)
                    {
                        data = data.Where(u => u.CreateTime>=param.BeginTime);
                    }
                    if (param.EndTime.HasValue)
                    {
                        data = data.Where(u => u.CreateTime<=param.EndTime);
                    }
                    
                    if (!string.IsNullOrWhiteSpace(param.EditUser))
                    {
                        data = data.Where(u => u.CreateUsername.Contains(param.EditUser));
                    }
                }

               
                var pagedList =
                    new PagedList<CommTestPlan>(data.OrderBy(d => d.CreateTime), model.PageIndex,
                        model.PageSize);
                var planTypes=_dictionaryService.GetDictionaryInfos(Global.PlanType);
                pagedList.AsParallel().ForEach(d =>
                {
                    var sysDictionaryInfo = planTypes.SingleOrDefault(item => item.CodeId == d.PlanId);
                    if (sysDictionaryInfo != null)
                        d.PlayTypeName = sysDictionaryInfo.Enname;
                }); 
                model.LoadPagedList(pagedList);
                model.Data = pagedList;
                return model;
            });
            return task;
        }

        //
        // GET: /CommTestPlan/Details/5

        public ActionResult Detail(decimal? id)
        {
            return RedirectToAction("Edit",new{id});
        }

        //
        // GET: /CommTestPlan/Create

        public ActionResult Create(decimal? areaId)
        {
            CommTestPlan testPlan = new CommTestPlan();
            if (areaId.HasValue)
            {
                AreaInfo area;
                if (areaId>10000)
                {
                    area= _areaService.GetAllAreas().SingleOrDefault(d => d.AreaId == areaId);
                }
                else
                { 
                    if (areaId>0&&areaId<100)
                    {
                         area= _areaService.GetAllAreas().SingleOrDefault(d => d.ProvinceId == areaId);
                    }
                    else
                    {
                        area=_areaService.GetAllAreas().SingleOrDefault(d => d.CityId == areaId);
                    }
                }
                 if (area != null)
                    {
                        testPlan.ProvinceId = area.ProvinceId;
                        testPlan.CityId = area.CityId;
                        testPlan.AreaId = area.AreaId;
                    }
            }
            else
            {
                testPlan.ProvinceId = 0;
            }
            var plans = _dictionaryService.GetDictionaryInfos(Global.PlanType).Select(d=>new SelectListItem() {Text=d.Enname,Value = d.CodeId.ToString()}).ToList();
            plans.First().Selected = true;
            ViewBag.PlanTypes = plans;
            return View(testPlan);
        } 

        //
        // POST: /CommTestPlan/Create

        [HttpPost]
        public ActionResult Create(CommTestPlan commtestplan)
        {
            if (ModelState.IsValid)
            {
                commtestplan.CreateTime = DateTime.Now;
                commtestplan.CreateUserid = CurrentUser.Auid;
                commtestplan.CreateUsername = CurrentUser.UserName;
                _commtestplanRepository.Insert(commtestplan);
                return RedirectToAction("Index");
            } else {
                return View(commtestplan);
			}
        }
        
        //
        // GET: /CommTestPlan/Edit/5
 
        public ActionResult Edit(decimal? id)
        {
            var model = _commtestplanRepository.GetById(id);
             ViewBag.PlanTypes = _dictionaryService.GetDictionaryInfos(Global.PlanType).Select(d=>new SelectListItem(){Text = d.Enname,Value = d.CodeId.ToString(),Selected =d.CodeId==model.PlanType }).ToList();
             return View(model);
        }

        //
        // POST: /CommTestPlan/Edit/5

        [HttpPost]
        public ActionResult Edit(CommTestPlan commtestplan)
        {
            if (ModelState.IsValid) {
                commtestplan.CreateTime = DateTime.Now;
                commtestplan.CreateUserid = CurrentUser.Auid;
                commtestplan.CreateUsername = CurrentUser.UserName;
              _commtestplanRepository.Update(commtestplan);
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

     
        //
        // POST: /CommTestPlan/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal? id)
        {
            _commtestplanRepository.Delete(id);

            return Json(1);
        }

        
    }
}

