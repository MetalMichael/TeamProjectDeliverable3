﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimetableSystem.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;

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
                foreach (var item in systemDB.Modules.ToList())    // pass only modules belonging to logged in user
                {
                    if (item.Department == User.Identity.Name)
                    {
                        modules.Add(item);
                    }
                }
            }

            if (!String.IsNullOrEmpty(moduleTitle))  // search filter
            {
                var mod = from m in systemDB.Modules
                          select m;

                if (!String.IsNullOrEmpty(moduleTitle))
                {
                    mod = mod.Where(s => s.ModuleTitle.Contains(moduleTitle));
                }
                ViewBag.moduleTitle = moduleTitle;
                return View(mod);
            }


            if (!String.IsNullOrEmpty(moduleCode))  // search filter
            {
                var mod = from m in systemDB.Modules 
                select m; 
 
                if (!String.IsNullOrEmpty(moduleCode)) 
                { 
                    mod = mod.Where(s => s.ModuleCode.Contains(moduleCode)); 
                }
                ViewBag.moduleCode = moduleCode;
                return View(mod); 
            }

            ViewBag.Department = User.Identity.Name;

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

            module.Department = module.Department;
            
            if (ModelState.IsValid)
            {
                bool moduleExists = true;  // check if module already exists
                foreach (var item in systemDB.Modules)
                {
                    if (item.ModuleCode == module.ModuleCode)
                        moduleExists = false;
                }

                if (moduleExists == false)    
                {
                    Module temp = systemDB.Modules.Find(module.ModuleCode);    // remove duplicate
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
        public ActionResult Edit(string code)
        {
            if (code == "")    // means "create new" was clicked
            {
                return CreateForm(new Module());
            }
            try
            {
                Module module = systemDB.Modules.Find(code);
                return CreateForm(module);
            }
            catch
            {
                ViewBag.Department = User.Identity.Name;
                return View();
            }
        
        }

        [HttpGet]
        public ActionResult Delete(string code)
        {
            try
            {
                Module module = systemDB.Modules.Find(code);
                //systemDB.Modules.Remove(module);
                
                systemDB.Entry(module).State = System.Data.EntityState.Deleted;
                systemDB.SaveChanges();
                return Redirect("/team09web/Module");  // no delete confirmation yet
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
            ViewBag.Department = User.Identity.Name;

            ViewBag.Department = User.Identity.Name;
            return View(module);
        }

    }
}