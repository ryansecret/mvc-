using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hsr.Core;
using Hsr.Data.Interface;
using Hsr.Model.Models;
using Hsr.Models;

namespace Hsr.Controllers
{   
    public class RyanController : BaseController
    {
		private readonly IRepository<Ryan> ryanRepository;
 
        public RyanController(IRepository<Ryan> ryanRepository)
        {
			this.ryanRepository = ryanRepository;
        }

        //
        // GET: /Ryan/
        public ViewResult Index()
        {
            return View(ryanRepository.Table);
        }

        //
        // GET: /Ryan/Details/5

        public ViewResult Details(string id)
        {
            return View(ryanRepository.GetById(id));
        }

        //
        // GET: /Ryan/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Ryan/Create

        [HttpPost]
        public ActionResult Create(Ryan ryan)
        {
            if (ModelState.IsValid)
            {
                ryanRepository.Insert(ryan);
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /Ryan/Edit/5
 
        public ActionResult Edit(string id)
        {
             return View(ryanRepository.GetById(id));
        }

        //
        // POST: /Ryan/Edit/5

        [HttpPost]
        public ActionResult Edit(Ryan ryan)
        {
            if (ModelState.IsValid)
            {
                ryanRepository.Update(ryan);
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /Ryan/Delete/5
 
        public ActionResult Delete(string id)
        {
            return View(ryanRepository.GetById(id));
        }

        //
        // POST: /Ryan/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            ryanRepository.Delete(id);
            
            return RedirectToAction("Index");
        }
 
    }
}

