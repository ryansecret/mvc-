using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hsr.Data.Interface;
using Hsr.Models;
using MVC.Controls;

namespace Hsr.Controllers
{
    public class ImportController : Controller
    {
        //
        // GET: /Import/
        private readonly IRepository<Datamapping> _datamappingRepository;
        private readonly IRepository<DatamappingColumn> _datamappingColumnRepository;

        public ImportController(IRepository<Datamapping> datamappingRepository, IRepository<DatamappingColumn> datamappingColumnRepository)
        {
            _datamappingRepository = datamappingRepository;
            _datamappingColumnRepository = datamappingColumnRepository;
        }


        public ActionResult Index(Datamappings model)
        {
            if (model.PageNumber <= 0)
            {
                model.PageNumber = 1;
            }
            if (model.PageSize <= 0)
            {
                model.PageSize = 3;
            }
            PagedList<Datamapping> pagedList = new PagedList<Datamapping>(_datamappingRepository.Table.OrderBy(d=>d.Id), model.PageIndex, model.PageSize);
            model.LoadPagedList(pagedList);
            model.Data = pagedList;
            return View(model);
        }

        public DatamappingColumns Columlist(int? id)
        {
            DatamappingColumns model = new DatamappingColumns();
            if (model.PageNumber <= 0)
            {
                model.PageNumber = 1;
            }
            if (model.PageSize <= 0)
            {
                model.PageSize = 3;
            }
            PagedList<DatamappingColumn> pagedList = new PagedList<DatamappingColumn>(_datamappingColumnRepository.Table.Where(p => p.Dataid == id).OrderBy(d => d.Id), model.PageIndex, model.PageSize);
            model.LoadPagedList(pagedList);
            model.Data = pagedList;
            return model;
        }

        public ActionResult Create()
        {
            ViewBag.list = null;
            return View(new Datamapping());
        }
        [HttpPost]
        public ActionResult Create(Datamapping model)
         {
            if (ModelState.IsValid)
            {
                _datamappingRepository.Insert(model);
                return RedirectToAction("Index");
            }
            
            return View(model);
        }

        public ActionResult CreateColumn(int id)
        {
            ViewBag.list = Columlist(id);
            return View(new Datamapping());
        }
        [HttpPost]
        public ActionResult CreateColumn(DatamappingColumn model)
        {
            _datamappingColumnRepository.Insert(model);
            return RedirectToAction("Create");
        }

        public ActionResult Edit(int id)
        {
            var data = _datamappingRepository.GetById(id);
            ViewBag.list = Columlist(id);
            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(Datamapping model)
        {
            _datamappingRepository.Update(model);
            return RedirectToAction("Index");
        }

        public ActionResult EditColumn(int id)
        {
            var data=_datamappingColumnRepository.GetById(id);
            return View(data);
        }
        [HttpPost]
        public ActionResult EditColumn(DatamappingColumn model)
        {
            _datamappingColumnRepository.Update(model);
            return RedirectToAction("Edit");
        }

        public ActionResult Delete(int id)
        {
            _datamappingRepository.Delete(id); 
            return View("Index");
        }

        public ActionResult DeleteColumn(int id)
        {
            _datamappingColumnRepository.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Detal(int id)
        {
            var data = _datamappingRepository.GetById(id);
            ViewBag.list = Columlist(id);
            return View(data);
        }
        public ActionResult DetalColumn(int id)
        {
            var data = _datamappingColumnRepository.GetById(id);
            return View(data);
        } 

    }
}
