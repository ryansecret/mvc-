using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hsr.Core;
using Hsr.Data.Interface;
using System.Data.Entity;
using System.Linq.Expressions;
using Hsr.Model.Models;
using Hsr.Models;
using MVC.Controls;

namespace Hsr.Controllers
{   
    public class ControllerFilterDataController : BaseController
    {
		private readonly IRepository<ControllerFilterData> testRepository;
		private readonly IRepository<ControllerFilterData> controllerfilterdataRepository;

	 
        public ControllerFilterDataController(IRepository<ControllerFilterData> testRepository, IRepository<ControllerFilterData> controllerfilterdataRepository)
        {
			this.testRepository = testRepository;
			this.controllerfilterdataRepository = controllerfilterdataRepository;
        }

        //
        // GET: /ControllerFilterData/

        public ViewResult Index(MessageFilterModel model)
        {
            if (model.PageNumber <= 0)
            {
                model.PageNumber = 1;
            }
            if (model.PageSize <= 0)
            {
                model.PageSize = 15;
            }
            PagedList<ControllerFilterData> pagedList =
                new PagedList<ControllerFilterData>(controllerfilterdataRepository.Table.OrderBy(d => d.MsgId), model.PageIndex, model.PageSize);
            model.LoadPagedList(pagedList);
            model.Data = pagedList;
            return View(model);
        }

        //
        // GET: /ControllerFilterData/Details/5

        public ViewResult Details(int? id)
        {
            return View(controllerfilterdataRepository.GetById(id));
        }

        //
        // GET: /ControllerFilterData/Create

        public ActionResult Create()
        {
            
            return View();
        } 

        //
        // POST: /ControllerFilterData/Create

        [HttpPost]
        public ActionResult Create(ControllerFilterData controllerfilterdata)
        {
            if (ModelState.IsValid) {
                
                controllerfilterdataRepository.Insert(controllerfilterdata);
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleTest = testRepository.Table;
				return View();
			}
        }
        
        //
        // GET: /ControllerFilterData/Edit/5
 
        public ActionResult Edit(int? id)
        {
            var data = controllerfilterdataRepository.GetById(id);
            SelectList selectList = new SelectList(testRepository.Table.Take(10).Select(d => new { method = d.MethodeName}).Distinct(), "method", "method");
          
            ViewBag.PossibleTest = selectList;
           // ViewBag.PossibleTest = controllerfilterdataRepository.Table.Take(10).Select(d => d.Test);
             return View(data);
        }

        //
        // POST: /ControllerFilterData/Edit/5

        [HttpPost]
        public ActionResult Edit(ControllerFilterData controllerfilterdata)
        {
            if (ModelState.IsValid) {
              controllerfilterdataRepository.Update(controllerfilterdata);
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleTest = testRepository.Table;
				return View();
			}
        }

        //
        // GET: /ControllerFilterData/Delete/5
 
        public ActionResult Delete(int? id)
        {
            return View(controllerfilterdataRepository.GetById(id));
        }

        //
        // POST: /ControllerFilterData/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            controllerfilterdataRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}

