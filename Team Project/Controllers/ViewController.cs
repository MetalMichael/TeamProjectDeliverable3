using System;
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

            ViewBag.Modules = new SelectList(db.Modules, "ModuleCode", "ModuleTitle");
            ViewBag.ModuleCodes = new SelectList(db.Modules, "ModuleCode", "ModuleCode");
            ViewBag.Department = User.Identity.Name;

            return View(requests);
        }
    }
}
