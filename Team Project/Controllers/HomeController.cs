using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using TimetableSystem.Models;

namespace TimetableSystem.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private TimetableSystemEntities db = new TimetableSystemEntities();

        public ActionResult Index()
        {
            ViewBag.Title = "Create new Request";
            return View();
        }

        [HttpPost]
        public ActionResult Index(Request request)
        {
            if (ModelState.IsValid)
            {
                db.Requests.Add(request);
                db.SaveChanges();
                return View();
            }

            ViewBag.Title = "Create new Request";
            return View();
        }

    }
}
