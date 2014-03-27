using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimetableSystem.Models;

namespace TimetableSystem.Controllers
{
    public class AvailabilityController : Controller
    {
        TimetableSystemEntities systemDB = new TimetableSystemEntities();

        public ActionResult Index()
        {

            var parkList = new List<String>();
            var parkQry = from p in systemDB.Parks
                          orderby p.ParkID
                          select p.ParkName;
            parkList.AddRange(parkQry);
            ViewBag.Park = parkList;

            var buildList = new List<String>();
            var buildQry = from b in systemDB.Buildings
                           orderby b.BuildingName
                           select b.BuildingName;
            buildList.AddRange(buildQry.Distinct());
            ViewBag.Building = buildList;

            var roomList = new List<String>();
            var roomQry = from r in systemDB.Rooms
                          orderby r.RoomCode
                          select r.RoomCode;
            roomList.AddRange(roomQry.Distinct());
            ViewBag.Room = roomList;
       
            
            /*string[] park = { "East", "Central", "West" };
            SelectList parkList = new SelectList(park);
            ViewBag.Park = parkList;
            string[] buildings = { "James France", "HazleGrave", "Schofield", "Brockington", "Edward Herbert Building", "Stewart Mason Building" };
            SelectList buildingList = new SelectList(buildings);
            ViewBag.Building = buildingList;
            string[] room = { "J001", "J002", "N001", "N002", "N003", "U020" };
            SelectList roomList = new SelectList(room);
            ViewBag.Room = roomList;*/

            int[] semester = { 1, 2 };
            SelectList semesterList = new SelectList(semester);
            ViewBag.Semester = semesterList;
            int[] week = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            SelectList weekList = new SelectList(week);
            ViewBag.Week = weekList;

            return View();
        }

    }
}
