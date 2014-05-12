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
            ViewBag.Department = User.Identity.Name;
            return View(requests);
        }
    }
}
