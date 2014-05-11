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
    [Authorize]
    public class HomeController : Controller
    {
        private TimetableSystemEntities db = new TimetableSystemEntities();

        public ActionResult Edit(int id)
        {
            Request request = db.Requests.Find(id);
            
            ViewBag.SelectedRooms = new List<String>();
            foreach(TimetableSystem.Models.RequestRoom room in request.RequestRooms)
            {
                ViewBag.SelectedRooms.Add(room.RoomID.ToString());
            }

            return CreateForm(request);
        }

        [HttpPost]
        public ActionResult Edit(Request request)
        {
            request = this.processRequest(request);
            
            if (ModelState.IsValid)
            {
                Request old = db.Requests.Find(request.RequestId);
                db.Requests.Remove(old);
                db.SaveChanges();
                
                db.Requests.Add(request);
                db.SaveChanges();
                ViewBag.Message = "Edited Successfully";
                return RedirectToAction("Index", "View");
            }

            return CreateForm(request);
        }

        public ActionResult Index()
        {
            return CreateForm(new Request());
        }

        [HttpPost]
        public ActionResult Index(Request request)
        {
            request = this.processRequest(request);

            if (ModelState.IsValid)
            {
                db.Requests.Add(request);
                db.SaveChanges();
                ViewBag.Message = "Request Created";
                return CreateForm(new Request());
            }

            return CreateForm(request);
        }

        private Request processRequest(Request request) {
            List<RequestWeek> weeks = new List<RequestWeek>();
            if (request.Weeks != null && request.Weeks.Count > 0)
            {
                foreach (var week in request.Weeks)
                {
                    RequestWeek rw = new RequestWeek(Convert.ToInt16(week));
                    weeks.Add(rw);
                }
            }
            else
            {
                ModelState.AddModelError("NoWeeks", "Request cannot be for no weeks");
            }
            request.RequestWeeks = weeks;
            request.Department = User.Identity.Name;

            List<RequestRoom> rooms = new List<RequestRoom>();
            if (request.Rooms != null && request.Rooms.Count > 0)
            {
                foreach (string room in request.Rooms)
                {
                    RequestRoom rr = new RequestRoom(Convert.ToInt16(room));
                    rooms.Add(rr);
                }
            }
            else
            {
                RequestRoom rr = new RequestRoom();
                rooms.Add(rr);
            }
            request.RequestRooms = rooms;

            request.Status = "Pending";
            return request;
        }

        private ActionResult CreateForm(Request request)
        {

            // Temp - code to setup the room features checkboxes
            string projector = "";
            projector += "<input checked='checked' class='check-box' id='Projector2' name='Projector2' type='checkbox' value='true'> <label for='" + request.Projector + "'>" + request.Projector + "</label>";
            string week1 = "", week2 = "", week3 = "";
            string weekNo;
            for (int x = 1; x < 6; x++)
            {
                weekNo = "Week" + x;
                week1 += "<input class='weekBox' id='" + weekNo + "' type='checkbox' name='Weeks' value='" + x + "' checked /><label for='" + weekNo + "'>" + x + "</label>";
            }
            for (int x = 6; x < 11; x++)
            {
                weekNo = "Week" + x;
                week2 += "<input class='weekBox' id='" + weekNo + "' type='checkbox' name='Weeks' value='" + x + "' checked /><label for='" + weekNo + "'>" + x + "</label>";
            }
            for (int x = 11; x < 16; x++)
            {
                weekNo = "Week" + x;
                if (x < 13)
                {
                    week3 += "<input class='weekBox' id='" + weekNo + "' type='checkbox' name='Weeks' value='" + x + "' checked /><label for='" + weekNo + "'>" + x + "</label>";
                }
                else
                {
                    week3 += "<input class='weekBox' id='" + weekNo + "' type='checkbox' name='Weeks' value='" + x + "' /><label for='" + weekNo + "'>" + x + "</label>";
                }
            }

            ViewBag.WeekCheckboxes1 = week1;
            ViewBag.WeekCheckboxes2 = week2;
            ViewBag.WeekCheckboxes3 = week3;

            //string features1 = "<input id='" + "' type='checkbox' value='true'>";
            ViewBag.Projector = projector;

            ViewBag.Title = "Create new Request";

            ViewBag.Modules = new SelectList(db.Modules, "ModuleCode", "ModuleTitle");
            ViewBag.ModuleCodes = new SelectList(db.Modules, "ModuleCode", "ModuleCode");
            ViewBag.Buildings = new SelectList(db.Buildings, "BuildingID", "BuildingName");
            ViewBag.RoomTypes = new SelectList(db.Types, "RoomTypeID", "RoomType");
            ViewBag.Parks = new SelectList(db.Parks, "ParkID", "ParkName");

            var times = new List<SelectListItem>();
            for (var x = 9; x <= 17; x++)
            {
                string y = x.ToString();
                int p = x - 8;
                if (y.Length == 1)
                {
                    y = "0" + y;
                }
                times.Add(new SelectListItem { Text = y + ":00 - Period: " + p, Value = y });
            }
            ViewBag.Times = times;

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

        public ActionResult Rooms()
        {
            int Students, ParkId, BuildingId;
            try {
                Students = Convert.ToInt16(Request.QueryString["students"]);
            } catch (FormatException) {
                Students = 0;
            }

            try {
                ParkId = Convert.ToInt16(Request.QueryString["park"]);
            } catch (FormatException) {
                ParkId = 0;
            }

            try {
                BuildingId = Convert.ToInt16(Request.QueryString["building"]);
            } catch (FormatException) {
                BuildingId = 0;
            }

            var rooms = db.Rooms.Where(a => a.Capacity >= Students);

            if (BuildingId != 0)
            {
                rooms = db.Rooms.Where(a => a.BuildingID == BuildingId).Where(a => a.Capacity >= Students);
            }
            else if (ParkId != 0)
            {
                rooms = db.Rooms.Where(a => a.Building.ParkID == ParkId).Where(a => a.Capacity >= Students);
            }

            string select = "<select class='room-select' name='Rooms'><option value='0'></option>";
            foreach (var room in rooms)
            {
                select += "<option value='" + room.RoomID + "'>" + room.RoomCode + "&nbsp;&nbsp;&nbsp;&nbsp;(Cap: " + room.Capacity + ")</option>";
            }
            select += "</select>";

            return Content(select);
        }

        public ActionResult Buildings()
        {
            int ParkId;
            
            try {
                ParkId = Convert.ToInt16(Request.QueryString["park"]);
            } catch (FormatException) {
                ParkId = 0;
            }

            System.Linq.IQueryable<TimetableSystem.Models.Building> buildings;
            if (ParkId != 0)
            {
                buildings = db.Buildings.Where(a => a.ParkID == ParkId);
            }
            else
            {
                buildings = db.Buildings;
            }

            string options = "<option value='0'></option>";
            foreach (var building in buildings)
            {
                options += "<option value='" + building.BuildingID + "'>" + building.BuildingName + "</option>";
            }

            return Content(options);
        }

    }
}
