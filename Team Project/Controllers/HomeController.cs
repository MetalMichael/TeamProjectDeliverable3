using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using TimetableSystem.Models;

namespace TimetableSystem.Controllers
{
    [HandleError]
    [Authorize]
    public class HomeController : Controller
    {
        private TimetableSystemEntities db = new TimetableSystemEntities();

        public ActionResult Index()
        {
            ViewBag.Title = "Create new Request";
            ViewBag.status = 0;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Request request)
        {
            if (ModelState.IsValid)
            {
                db.Requests.Add(request);
                db.SaveChanges();

                ViewBag.status = 2;
                return View();
            }

            ViewBag.status = 1;
            return View();
        }
    }
}
