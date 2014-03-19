using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimetableSystem.Models;

namespace TimetableSystem.Controllers
{
    [Authorize]
    public class ModuleController : Controller
    {
        TimetableSystemEntities systemDB = new TimetableSystemEntities();
        //
        // GET: /Module/

        public ActionResult Index()
        {
            List<Module> modules = new List<Module>();
            if (systemDB.Modules != null)
            {
                modules = systemDB.Modules.ToList();
            }
            return View(modules);
        }

        //
        // GET: /Module/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Module/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Module/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Module/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Module/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Module/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Module/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
