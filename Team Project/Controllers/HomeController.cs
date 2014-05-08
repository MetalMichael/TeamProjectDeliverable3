using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using TimetableSystem.Models;
using System.Text;
using System.Reflection;

namespace TimetableSystem.Controllers
{
    [HandleError]
    [Authorize]
    public class HomeController : Controller
    {
        private TimetableSystemEntities db = new TimetableSystemEntities();

        public ActionResult Edit(int id)
        {
            Request request = db.Requests.Find(id);
            return CreateForm(request);
        }

        public ActionResult Index()
        {
            return CreateForm(new Request());
        }

        [HttpPost]
        public ActionResult Index(Request request)
        {
            List<RequestWeek> weeks = new List<RequestWeek>();
            if (request.Weeks != null && request.Weeks.Count > 0)
            {
                foreach (var week in request.Weeks)
                {
                    RequestWeek rw = new RequestWeek(Convert.ToInt16(week));
                    weeks.Add(rw);
                }
            }
            request.RequestWeeks = weeks;
            request.Department = User.Identity.Name;

            if (ModelState.IsValid)
            {
                db.Requests.Add(request);
                db.SaveChanges();
                return CreateForm(new Request());
            }

            System.Diagnostics.Debug.WriteLine("Could not create");

            var errors = ModelState.Where(a => a.Value.Errors.Count > 0)
                .Select(b => new { b.Key, b.Value.Errors })
                .ToArray();

            foreach (var modelStateErrors in errors)
            {
                System.Diagnostics.Debug.WriteLine("...Errored When Binding.", modelStateErrors.Key.ToString());
            }

            return CreateForm(request);
        }

        private ActionResult CreateForm(Request request)
        {
            ViewBag.Title = "Create new Request";

            ViewBag.Modules = new SelectList(db.Modules, "ModuleCode", "ModuleTitle");
            ViewBag.ModuleCodes = new SelectList(db.Modules, "ModuleCode", "ModuleCode");
            ViewBag.Buildings = new SelectList(db.Buildings, "BuildingID", "BuildingName");
            ViewBag.Rooms = new SelectList(db.Rooms, "RoomID", "RoomCode");
            ViewBag.RoomTypes = new SelectList(db.Types, "RoomTypeID", "RoomType");

            var times = new List<String>();
            for (var x = 9; x <= 17; x++)
            {
                string y = x.ToString();
                if (y.Length == 1)
                {
                    y = "0" + y;
                }
                times.Add(y + ":00");
            }
            ViewBag.Times = new SelectList(times);

            var lengths = new List<String>();
            for (var x = 1; x < 10; x++)
            {
                lengths.Add(x.ToString());
            }
            ViewBag.Lengths = new SelectList(lengths);

            ViewBag.Days = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

            ViewBag.WeekCheckboxes = "";
            for (int x = 1; x < 16; x++)
            {
                if (x <= 12)
                {
                    ViewBag.WeekCheckboxes += "<label for='Week" + x + "'>" + x + "</label> <input id='Week" + x + "' type='checkbox' name='Weeks' value='" + x + "' checked />";
                }
                else
                {
                    ViewBag.WeekCheckboxes += "<label for='Week" + x + "'>" + x + "</label> <input id='Week" + x + "' type='checkbox' name='Weeks' value='" + x + "' />";
                }
            }

            ViewBag.Department = User.Identity.Name;

            return View(request);
        }

    }
}
