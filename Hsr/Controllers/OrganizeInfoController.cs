#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hsr.Data.Interface;
using Hsr.Model.Models;
using Hsr.Model.Models.ViewModel;
using Hsr.Service;
using Hsr.Service.Iservice;
using MVC.Controls;

#endregion

namespace Hsr.Controllers
{
    public class OrganizeInfoController : HsrBaseController
    {
        private readonly IAreaService _areaService;
        private readonly IDictionaryService _dictionaryService;
        private readonly IRepository<OrganizeInfo> _organizeinfo;

        public OrganizeInfoController(IRepository<OrganizeInfo> organizeinfo, IAreaService areaService,
            IDictionaryService dictionaryService)
        {
            _organizeinfo = organizeinfo;
            _areaService = areaService;
            _dictionaryService = dictionaryService;
        }

        public ActionResult GetArea(int? pid) //  显示区域信息（Tree）
        {
            var areas = new List<AreaVm>();
            bool isOnlyArea = false;
            if (!pid.HasValue) //pid为空的话  只返回  全国
            {
                var country = new AreaVm();
                country.id = "0";
                country.text = "全国";
                areas.Add(country);
            }
            else
            {
                if (pid.Value > 0 && pid.Value < 100) // 再次请求  满足这个条件  返回 所有市 （市的pid都是在1-100之间）
                {
                    List<AreaVm> citys = _areaService.GetCitys(pid);
                    areas.AddRange(citys);
                }
                else
                {
                    if (pid == 0)
                    {
                        List<AreaVm> pros = _areaService.GetProvince(); //  否则 并且满足这个条件 返回所有省

                        areas.AddRange(pros);
                    }
                    else
                    {
                        areas.AddRange(_areaService.GetAreas(pid)); //  再次请求 以上条件都不满足 返回所有区

                        isOnlyArea = true; // isOnlyArea为true  说明children为false
                    }
                }
            }
            if (!isOnlyArea)
            {
                areas.ForEach(d => d.children = true);
            }
            return Json(areas, JsonRequestBehavior.AllowGet); //返回json数据
        }

        public ActionResult GetOrgTree()
        {
            var orgs = _organizeinfo.TableNoTracking.ToList().ToOrgVms();

            return Json(OrgToJsNodes(orgs), JsonRequestBehavior.AllowGet);
        }


        private TreeJsonNode OrgToJsNodes(List<OrganizeInfoVm> roles, bool setDisable = false, decimal? pOrgId = null)
        {
            if (!roles.Any())
            {
                return new TreeJsonNode();
            }
            var pid = pOrgId.HasValue ? pOrgId.Value : 0;
            var topOrgs = roles.Where(d => d.OrgPid == pid).Select(d => d.ToJsNode(setDisable)).ToList();

            foreach (var topOrg in topOrgs)
            {
                AssemblyChildNodes(topOrg, roles);
            }
            if (topOrgs.Any())
            {
                var treeJsonNode = new TreeJsonNode();
                treeJsonNode.pid = "-1";
                treeJsonNode.children = topOrgs;
                treeJsonNode.id = "0";
                treeJsonNode.text = !setDisable ? "所有机构" : "所属组织";
                treeJsonNode.icon = "/Images/home.png";
                treeJsonNode.state = new StateType();

                treeJsonNode.state.opened = true;
                return treeJsonNode;
            }
            return null;
        }

        private void AssemblyChildNodes(TreeJsonNode role, List<OrganizeInfoVm> allorgs)
        {
            var childs = allorgs.Where(d => d.OrgPid == int.Parse(role.id)).ToList();

            if (childs.Any())
            {
                role.children = childs.Select(d => d.ToJsNode()).ToList();
                foreach (var child in role.children)
                {
                    AssemblyChildNodes(child, allorgs);
                }
            }
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetOrgnization(int? orgPid) // id不为空  返回部门  否则返回公司
        {
            List<OrganizeInfoVm> orgs = null;
            List<OrganizeInfo> list = null;
            bool setDisable = false;
            if (orgPid != null)
            {
                list = _organizeinfo.Table.Where(d => d.OrgPid == orgPid && d.Category != "1").ToList();
                setDisable = true;
            }
            else
            {
                list = _organizeinfo.Table.Where(d => d.Category == "1").ToList();
            }
            orgs = list.ToOrgVms();
            return Json(OrgToJsNodes(orgs, setDisable, orgPid), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Index(int? orgid, int? areaid, OrganizeInfoVms model)
        {
            var data = await GetList(orgid, areaid, model);
            return View(data);
        }

        public async Task<ActionResult> GetOrgList(int? orgid, int? areaid, OrganizeInfoVms model)
        {
            var menus = await GetList(orgid, areaid, model);

            return PartialView("OrgList", menus);
        }

        private Task<OrganizeInfoVms> GetList(int? pid, int? areaid, OrganizeInfoVms model)
        {
            var data = Task.Factory.StartNew(() =>
            {
                var org = _organizeinfo.TableNoTracking;
                if (model.PageNumber <= 0)
                {
                    model.PageNumber = 1;
                }
                if (model.PageSize <= 0)
                {
                    model.PageSize = 2;
                }
                if (pid.HasValue && pid.Value > 0)
                {
                    org = org.Where(d => d.OrgPid == pid);
                }
                if (areaid.HasValue && areaid.Value > 0)
                {
                    if (areaid.Value > 10000)
                    {
                        org = org.Where(d => d.AreaId == areaid.Value);
                    }
                    else
                    {
                        if (areaid.Value > 100)
                        {
                            org = org.Where(d => d.CityId == areaid.Value);
                        }
                        else
                        {
                            org = org.Where(d => d.ProvinceId == areaid.Value);
                        }
                    }
                }
                var pagedList = new PagedList<OrganizeInfoVm>(org.ToList().ToOrgVms(), model.PageIndex, model.PageSize);
                model.LoadPagedList(pagedList);
                model.Data = pagedList;
                return model;
            });
            return data;
        }


        public ViewResult Details(decimal? id) //    显示详细
        {
            return View(_organizeinfo.GetById(id));
        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal? orgId, decimal? orgPid) //  删除
        {
            var child = _organizeinfo.Table.Where(d => d.OrgPid == orgId).ToList();

            if (child.Any())
            {
                foreach (var chil in child)
                {
                    _organizeinfo.Delete(chil.OrgId);
                }
            }
            if (_organizeinfo.Delete(orgId))
            {
                return Json("1");
            }
            return Json("0");
        }


        public ActionResult Edit(decimal? id)
        {
            var org = _organizeinfo.GetById(id).ToOrgVm();
            var pOrg = _organizeinfo.GetById(org.OrgPid).ToOrgVm();


            var prvinces =
                _areaService.GetProvince()
                    .Select(
                        d =>
                            new SelectListItem
                            {
                                Text = d.text,
                                Value = d.id,
                                Selected = d.id.ToString() == org.ProvinceId.ToString()
                            })
                    .ToList();
            var citys =
                _areaService.GetCitys(org.ProvinceId)
                    .Select(
                        d =>
                            new SelectListItem
                            {
                                Text = d.text,
                                Value = d.id,
                                Selected = d.id.ToString() == org.CityId.ToString()
                            })
                    .ToList();
            ;
            var areas =
                _areaService.GetAreas(org.CityId).Select(d => new SelectListItem {Text = d.text, Value = d.id}).ToList();
            ;
            ViewBag.Province = prvinces;
            ViewBag.Citys = citys;

            ViewBag.Areas = areas;
            ViewBag.OrgTypes =
                _dictionaryService.GetDictionaryInfos(Global.OrgType)
                    .Select(
                        d =>
                            new SelectListItem
                            {
                                Text = d.Chname,
                                Value = d.CodeId.ToString(),
                                Selected = d.CodeId == org.OrgType
                            })
                    .ToList();
            org.OrgPName = org.OrgPid != 0 ? pOrg.OrgName : "所有机构";
            return View(org);
        }

        //
        // POST: /Sysrole/Edit/5

        [HttpPost]
        public ActionResult Edit(OrganizeInfoVm org)
        {
            if (ModelState.IsValid)
            {
                org.CreateTime = DateTime.Now;
                org.CreateUserid = "1";
                _organizeinfo.Update(org.ToOrg());

                return RedirectToAction("Index", new {orgPid = org.OrgPid});
            }
            return View(org);
        }


        public ActionResult Detail(decimal? id)
        {
            return RedirectToAction("Edit", new {id});
        }

        /// <summary>
        ///     Creates the specified org pid.
        /// </summary>
        /// <param name="orgPid">The org pid.</param>
        /// <param name="orgPname">The org pname.</param>
        /// <param name="isDept">if set to <c>true</c> 是否部门.</param>
        /// <returns></returns>
        public ActionResult Create(int? orgPid, bool isDept)
        {
            var pOrg = _organizeinfo.TableNoTracking.FirstOrDefault(d => d.OrgId == orgPid);

            var org = new OrganizeInfoVm {OrgPid = orgPid, OrgPName = pOrg == null ? null : pOrg.OrgName};
            org.Category = isDept ? "2" : "1";
            List<AreaVm> prvince = _areaService.GetProvince();
            ViewBag.Province = prvince.Select(d => new SelectListItem {Text = d.text, Value = d.id}).ToList();
            var provinceKey = pOrg == null ? CurrentUser.ProvinceId : pOrg.ProvinceId;
            var cityKey = pOrg == null ? CurrentUser.CityId : pOrg.CityId;
            var areKey = pOrg == null ? CurrentUser.AreaId : pOrg.AreaId;

            var citys =
                _areaService.GetCitys(provinceKey)
                    .Select(
                        d =>
                            new SelectListItem
                            {
                                Text = d.text,
                                Value = d.id,
                                Selected = pOrg != null && d.id == pOrg.CityId.ToString()
                            })
                    .ToList();
            ViewBag.Citys = citys;
            ViewBag.Areas =
                _areaService.GetAreas(cityKey)
                    .Select(
                        d =>
                            new SelectListItem
                            {
                                Text = d.text,
                                Value = d.id,
                                Selected = pOrg != null && d.id == pOrg.AreaId.ToString()
                            })
                    .ToList();

            org.ProvinceId = provinceKey;
            org.CityId = cityKey;
            org.AreaId = areKey;

            ViewBag.OrgTypes =
                _dictionaryService.GetDictionaryInfos(Global.OrgType)
                    .Select(d => new SelectListItem {Text = d.Chname, Value = d.CodeId.ToString()})
                    .ToList();
            return View(org);
        }

        // POST: /Sysrole/Create
        [HttpPost]
        public ActionResult Create(OrganizeInfoVm org)
        {
            try
            {
                org.CreateTime = DateTime.Now;
                org.CreateBy = CurrentUser.UserName;
                org.CreateUserid = CurrentUser.Auid;

                _organizeinfo.Insert(org.ToOrg());
                return Json("1");
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }
    }
}