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
            var parkQry = from p in systemDB.Parks
                          orderby p.ParkID
                          select p.ParkName;
            SelectList parkList = new SelectList(parkQry);
            ViewBag.Park = parkList;

            var buildQry = (from b in systemDB.Buildings
                           orderby b.BuildingName
                           select b.BuildingName).Distinct();
            SelectList buildList = new SelectList(buildQry);
            ViewBag.Building = buildList;

            var roomQry = from r in systemDB.Rooms
                          orderby r.RoomCode
                          select r;
            List<string> tempList = new List<string>();
            foreach(Room r in roomQry)
            {
                tempList.Add(r.RoomCode + " [" + r.Capacity + "]");
            }
            SelectList roomList = new SelectList(tempList);
            ViewBag.Room = roomList;

            int[] semester = { 1, 2 };
            SelectList semesterList = new SelectList(semester);
            ViewBag.Semester = semesterList;
            int[] week = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            SelectList weekList = new SelectList(week);
            ViewBag.Week = weekList;

            var parksQry = from p in systemDB.Parks
                           select p;
            ViewBag.Temp = parksQry;

            return View();
        }
        public string parkSelected(string parkName)
        {
            //Need to catch error here when changing back to '' on park dropdown
            string buildings = "";
            string rooms = "";
            string returnStr = "";
            if (parkName == "")
            {
                //resetting buildings when "No Preference selected"
                var buildQry = (from b in systemDB.Buildings
                                orderby b.BuildingName
                                select b.BuildingName).Distinct();
                foreach (string b in buildQry)
                {
                    buildings += b + ';';
                }
                var roomQry = from r in systemDB.Rooms
                              select r;
                foreach (Room r in roomQry)
                {
                    rooms += r.RoomCode + " [" + r.Capacity.ToString() + "];";
                }
            }
            else
            {
                //Getting buildings
                var parkID = (from p in systemDB.Parks
                              where p.ParkName == parkName
                              select p.ParkID).Single();
                var buildQry = (from b in systemDB.Buildings
                                where b.ParkID == parkID
                                orderby b.BuildingName
                                select b.BuildingName).Distinct();
                foreach (string b in buildQry)
                {
                    buildings += b + ';';
                }

                //Getting rooms
                var buildIDs = (from b in systemDB.Buildings
                                where b.ParkID == parkID
                                orderby b.BuildingName
                                select b.BuildingID).Distinct();
                foreach (int b in buildIDs)
                {
                    var roomQry = from r in systemDB.Rooms
                                  where r.BuildingID == b
                                  select r;
                    foreach (Room r in roomQry)
                    {
                        rooms += r.RoomCode + " [" + r.Capacity.ToString() + "];";
                    }
                }
            }
            buildings = buildings.Substring(0, buildings.Length - 1);
            rooms = rooms.Substring(0, rooms.Length - 1);
            returnStr = buildings + "!" + rooms;

            return returnStr;
        }
        public string buildingSelected(string buildingName)
        {
            string rooms = "";
            
            if (buildingName == "")
            {
                var roomQry = from r in systemDB.Rooms
                              select r;
                foreach (Room r in roomQry)
                {
                    rooms += r.RoomCode + " [" + r.Capacity.ToString() + "];";
                }
            }
            else
            {
                var buildID = (from b in systemDB.Buildings
                               where b.BuildingName == buildingName
                               select b.BuildingID).First();
                var roomQry = from r in systemDB.Rooms
                              where r.BuildingID == buildID
                              select r;
                foreach (Room r in roomQry)
                {
                    rooms += r.RoomCode + " [" + r.Capacity.ToString() + "];";
                }
            }
            rooms = rooms.Substring(0, rooms.Length - 1);

            return rooms;
        }

    }
}
