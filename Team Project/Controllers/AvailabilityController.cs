using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimetableSystem.Controllers
{
    public class AvailabilityController : Controller
    {
        //
        // GET: /Availability/

        public ActionResult Index()
        {
            int[] semester = { 1, 2 };
            SelectList semesterList = new SelectList(semester);
            ViewBag.Semester = semesterList;
            string[] park = { "East", "Central", "West" };
            SelectList parkList = new SelectList(park);
            ViewBag.Park = parkList;
            string[] buildings = { "James France", "HazleGrave", "Schofield", "Brockington", "Edward Herbert Building", "Stewart Mason Building" };
            SelectList buildingList = new SelectList(buildings);
            ViewBag.Building = buildingList;
            string[] room = { "J001", "J002", "N001", "N002", "N003", "U020" };
            SelectList roomList = new SelectList(room);
            ViewBag.Room = roomList;
            int[] week = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            SelectList weekList = new SelectList(week);
            ViewBag.Week = weekList;
            return View();
        }

    }
}
