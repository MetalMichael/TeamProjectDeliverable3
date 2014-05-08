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
        public ActionResult Index(string moduleCode, string moduleTitle)
        {
            List<Module> allModules = new List<Module>();
            allModules = systemDB.Modules.ToList();     // temporary

            List<Module> modules = new List<Module>();
           
            if (systemDB.Modules != null)   // check modules exist
            {
                foreach (var item in allModules)    // pass only modules belonging to logged in user
                {
                    if (item.Department == User.Identity.Name)
                    {
                        modules.Add(item);
                    }
                }
            }

            if (!String.IsNullOrEmpty(moduleCode))
            {
                var mod = from m in systemDB.Modules 
                select m; 
 
                if (!String.IsNullOrEmpty(moduleCode)) 
                { 
                    mod = mod.Where(s => s.ModuleCode.Contains(moduleCode)); 
                } 
 
                return View(mod); 
            }

            return View(modules);

        }

        [HttpPost]
        public ActionResult Index(Module module)
        {
            module.Department = User.Identity.Name; // set department automatically

            if (module.ModuleCode == null || module.ModuleTitle == null)
            {
                //return RedirectToAction("Index");
                //return CreateForm(new Module());    // triggers IEnumerable error
            }

            if (ModelState.IsValid)
            {
                bool moduleExists = true;  // check if module already exists
                foreach (var item in systemDB.Modules)
                {
                    if (item.ModuleID == module.ModuleID)
                        moduleExists = false;
                }

                if (moduleExists == false)    
                {
                    Module temp = systemDB.Modules.Find(module.ModuleID);    // remove duplicate
                    systemDB.Entry(temp).State = System.Data.EntityState.Deleted;     
                }
                systemDB.Modules.Add(module);
                systemDB.SaveChanges();
                return RedirectToAction("Index");  // if successful, return to the list of modules
            }
            return RedirectToAction("Index");
            //return CreateForm(module);  // if unsuccessful, provide the filled out form again
        }

        //public ActionResult Index(IEnumerable<Module> moduleList)
        //{
        //    return View();
        //}

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

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                Module module = systemDB.Modules.Find(id);
                //systemDB.Modules.Remove(module);
                
                systemDB.Entry(module).State = System.Data.EntityState.Deleted;
                systemDB.SaveChanges();
                return Redirect("/Module");  // no delete confirmation yet
            }
            catch
            {
                return RedirectToAction("Index");   
            }
        }

        public ActionResult CreateForm(Module module)
        {
            ViewBag.Title = "Create New Module";

            string moduleCode = module.ModuleCode;
            try
            {
                moduleCode.Replace(" ", string.Empty);
            }
            catch { }

            string moduleTitle = module.ModuleTitle;
            try
            {
                moduleTitle.Replace(" ", string.Empty);
            }
            catch { }

            ViewBag.ModuleCode = moduleCode;
            ViewBag.ModuleTitle = moduleTitle;

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