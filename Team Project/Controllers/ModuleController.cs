﻿using System;
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
        private TimetableSystemEntities systemDB = new TimetableSystemEntities();

        //
        // GET: /Module/

        [HttpGet]
        public ActionResult Index()
        {
            List<Module> modules = new List<Module>();
            if (systemDB.Modules != null)
            {
                modules = systemDB.Modules.ToList();
            }
            return View(modules);
        }

        [HttpPost]
        public ActionResult Index(Module module)
        {
            module.Department = "1";    // temporary
            // add extra code to make input fit into the database?
            if (ModelState.IsValid)
            {
                
                systemDB.Modules.Add(module);
                systemDB.SaveChanges();
                return RedirectToAction("Index");  // if successful, return to the list of modules
            }
            return CreateForm(module);  // if unsuccessful, provide the filled out form again
        }

        //
        // GET: /Module/Details/5

        public ActionResult Details(int id)
        {
            return View();  //  needs doing
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)    // means "create new" was clicked
            {
                return CreateForm(new Module());
            }
            try
            {
                Module module = systemDB.Modules.Find(id);
                return CreateForm(module);
            }
            catch
            {
                return View();
            }
        
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Module module = systemDB.Modules.Find(id);
                systemDB.Entry(module).State = System.Data.EntityState.Deleted;

                return RedirectToAction("Index");   // no delete confirmation yet
            }
            catch
            {
                return RedirectToAction("Index");   
            }
        }

        public ActionResult CreateForm(Module module)
        {
            ViewBag.Title = "Create New Module";
            ViewBag.ModuleCode = module.ModuleCode;
            ViewBag.ModuleTitle = module.ModuleTitle;

            return View(module);
        }

        //
        // GET: /Module/Create

        //[HttpGet]   // not currently in use
        //public ActionResult Create()
        //{
        //    return CreateForm(new Module());
        //}

        //
        // POST: /Module/Create

        //[HttpPost]  // not currently in use
        //public ActionResult Create(Module module)
        //{
        //    try
        //    {
        //        systemDB.Modules.Add(module);
        //        // add the module to the db

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //
        // GET: /Module/Edit/5

        //public ActionResult Edit()  // edit with no input, i.e. "create"
        //{
        //    return CreateForm(new Module());
        //}

        //
        // POST: /Module/Edit/5

        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //
        // GET: /Module/Delete/5

        //public ActionResult Delete(int id)  // may be unnecessary
        //{
        //    return View(systemDB.Modules.Find(id));
        //}

        //
        // POST: /Module/Delete/5

    }
}