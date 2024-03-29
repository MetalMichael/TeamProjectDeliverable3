﻿using System;
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

        public ActionResult Autofill()
        {
            Request request = new Request();

            try {
                string weeks = Request.QueryString["Weeks"];
                request.Weeks = weeks.Split('|').Select(s => int.Parse(s)).ToList();
            } catch (Exception) {}

            ViewBag.SelectedRooms = new List<String>();
            try {
                string room = Request.QueryString["Room"];
                ViewBag.SelectedRooms.Add(room);
            } catch (FormatException) { }

            try {
                string day = Request.QueryString["Day"];
                request.Day = day;
            } catch (FormatException) { }

            try {
                int start = Convert.ToInt16(Request.QueryString["StartTime"]);
                request.StartTime = start;
            } catch (FormatException) { }

            return CreateForm(request);
        }

        [HttpPost]
        public ActionResult AutoFill(Request request)
        {
            request = this.processRequest(request);

            if (ModelState.IsValid)
            {
                db.Requests.Add(request);
                db.SaveChanges();
                ViewBag.Message = "Request Created";
                return CreateForm(new Request());
            }

            return RedirectToAction("Index");
        }

        public ActionResult ReSubmit(int id)
        {
            Request request = db.Requests.Find(id);
            return CreateForm(request);
        }

        [HttpPost]
        public ActionResult ReSubmit(Request request)
        {
            request = this.processRequest(request);

            if (ModelState.IsValid)
            {
                db.Requests.Add(request);
                db.SaveChanges();
                ViewBag.Message = "ReSubmitted Successfully";
                return RedirectToAction("Index", "View");
            }

            return RedirectToAction("Index");
        }

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

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Request request = db.Requests.Find(id);
            db.Requests.Remove(request);
            db.SaveChanges();

            ViewBag.Message = "Request Deleted";
            return RedirectToAction("Index", "View");
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
            List<AcceptedRoom> rooms2 = new List<AcceptedRoom>();
            if (request.Rooms != null && request.Rooms.Count > 0)
            {
                foreach (string room in request.Rooms)
                {
                    RequestRoom rr = new RequestRoom(Convert.ToInt16(room));
                    AcceptedRoom ar = new AcceptedRoom(Convert.ToInt16(room));
                    rooms.Add(rr);
                    rooms2.Add(ar);
                }
            }
            else
            {
                RequestRoom rr = new RequestRoom();
                rooms.Add(rr);
            }
            request.RequestRooms = rooms;

            if (request.AdHoc)
            {
                request.Round = 0;
                request.Status = "Accepted";
                request.AcceptedRoom = (int)rooms[0].RoomID;
                request.AcceptedRooms = rooms2;
            }
            else
            {
                request.Round = 2;
                request.Status = "Pending";
            }

            return request;
        }

        private ActionResult CreateForm(Request request)
        {
            string week1 = "", week2 = "", week3 = "";
            string weekNo;

            if (request.Weeks == null)
            {
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
            }
            else
            {
                string checkd;
                for (int x = 1; x < 6; x++)
                {
                    checkd = (request.Weeks.IndexOf(x) != -1) ? "checked" : "";
                    weekNo = "Week" + x;
                    week1 += "<input class='weekBox' id='" + weekNo + "' type='checkbox' name='Weeks' value='" + x + "' " + checkd + " /><label for='" + weekNo + "'>" + x + "</label>";
                }
                for (int x = 6; x < 11; x++)
                {
                    checkd = (request.Weeks.IndexOf(x) != -1) ? "checked" : "";
                    weekNo = "Week" + x;
                    week2 += "<input class='weekBox' id='" + weekNo + "' type='checkbox' name='Weeks' value='" + x + "' " + checkd + " /><label for='" + weekNo + "'>" + x + "</label>";
                }
                for (int x = 11; x < 16; x++)
                {
                    checkd = (request.Weeks.IndexOf(x) != -1) ? "checked" : "";
                    weekNo = "Week" + x;
                    week3 += "<input class='weekBox' id='" + weekNo + "' type='checkbox' name='Weeks' value='" + x + "' " + checkd + " /><label for='" + weekNo + "'>" + x + "</label>";
                }
            }
            //ViewBag.Projector = projector;

            ViewBag.WeekCheckboxes1 = week1;
            ViewBag.WeekCheckboxes2 = week2;
            ViewBag.WeekCheckboxes3 = week3;

            ViewBag.Modules = new SelectList(db.Modules.OrderBy(m => m.ModuleTitle), "ModuleCode", "ModuleTitle");
            ViewBag.ModuleCodes = new SelectList(db.Modules.OrderBy(m => m.ModuleCode), "ModuleCode", "ModuleCode");
            ViewBag.Buildings = new SelectList(db.Buildings.OrderBy(m => m.BuildingName), "BuildingID", "BuildingName");
            ViewBag.RoomTypes = new SelectList(db.Types.OrderBy(m => m.RoomType), "RoomTypeID", "RoomType");
            ViewBag.Parks = new SelectList(db.Parks.OrderBy(m => m.ParkName), "ParkID", "ParkName");

            var times = new List<SelectListItem>();
            for (var x = 1; x <= 9; x++)
            {
                string y = x.ToString();
                int t = x + 8;
                times.Add(new SelectListItem { Text = "Period " + y + " : (" + t + ":00 - " + t + ":50)" , Value = y });
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

            int[] semester = { 1, 2 };
            SelectList semesterList = new SelectList(semester);
            ViewBag.Semesters = semesterList;

            return View("Index", request);
        }

        public ActionResult Rooms()
        {
            //GET vars
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

            String[] facilities = new string[] {};
            string facilTemp;
            try {
                facilTemp = Request.QueryString["facilities"];
                if (facilTemp != "")
                {
                    facilities = facilTemp.Split('|');
                }
            } catch (Exception) { }

            int RoomTypeID;
            try
            {
                RoomTypeID = Convert.ToInt16(Request.QueryString["roomType"]);
            } catch (FormatException) {
                RoomTypeID = 0;
            }


            //DB Stuff
            var rooms = db.Rooms.Where(a => a.Capacity >= Students);

            if (BuildingId != 0)
            {
                rooms = rooms.Where(a => a.BuildingID == BuildingId);
            }
            if (ParkId != 0)
            {
                rooms = rooms.Where(a => a.Building.ParkID == ParkId);
            }
            if (RoomTypeID != 0)
            {
                rooms = rooms.Where(a => a.RoomType.RoomTypeID == RoomTypeID);
            }

            //Output
            string select = "<select class='room-select' name='Rooms'><option value='0'></option>";
            bool featureExists = false;
            bool fails = false;
            foreach (Room room in rooms)
            {
                featureExists = false;
                fails = false;
                if (facilities.Length > 0)
                {
                    foreach(string facility in facilities)
                    {
                        foreach(RoomFacility rf in room.RoomFacilities)
                        {
                            if (rf.Facility.FacilityName == facility)
                            {
                                featureExists = true;
                            }
                        }
                        if (!featureExists)
                        {
                            fails = true;
                            break;
                        }
                    }
                }
                if (!fails)
                {
                    select += "<option value='" + room.RoomID + "'>" + room.RoomCode + "&nbsp;&nbsp;&nbsp;&nbsp;(Cap: " + room.Capacity + ")</option>";
                }
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
