﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimetableSystem.Models;

namespace TimetableSystem.Controllers
{

    [Authorize]
    public class ViewController : Controller
    {
        TimetableSystemEntities db = new TimetableSystemEntities();
        //
        // GET: /View/

        public ActionResult Index()
        {
            List<Request> requests = new List<Request>();
            if (db.Requests != null) {
                requests = db.Requests.ToList();
            }

            ViewBag.Department = User.Identity.Name;

            ViewBag.Modules = new SelectList(db.Modules.OrderBy(m => m.ModuleTitle), "ModuleCode", "ModuleTitle");
            ViewBag.ModuleCodes = new SelectList(db.Modules.OrderBy(m => m.ModuleCode), "ModuleCode", "ModuleCode");

            return View(requests);
        }
    }
}
