using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimetableSystem.Models;

namespace TimetableSystem.Controllers
{
    [Authorize]
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

        //*************PARK SELECTED METHOD**************
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


        //********BUILDING SELECTED METHOD ************
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

        //**********GET AVAILABILITY METHOD***********
        public string getAvailability(string parkName, string buildingName, string roomCode, int semester, int week, int capacity)
        {
            int max = 0; //max number of rooms
            int available = 0; //number of available rooms
            string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            string day, htmlClass;
            string[] times = {"9:00-9:50", "10:00-10:50", "11:00-11:50", "12:00-12:50", "13:00-13:50", "14:00-14:50", "15:00-15:50",
                        "16:00-16:50", "17:00-17:50"};
            int[] periods = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int period;

            string html = "<table><tr><th><p style='text-align: center'>-</p></th>";
            for (int i = 0; i < 9; i++)
            {
                html += "<th>" + times[i] + "</th>";
            }
            html += "</tr>";

            var roomsWeeks = from w in systemDB.RequestWeeks
                             where (w.Week == week)
                             select w.RequestId;
            
            
            if (roomCode != "No" && roomCode != "")
            {
                
                var roomID = (from r in systemDB.Rooms
                              where r.RoomCode == roomCode
                              select r.RoomID).Single();
                max = (from r in systemDB.Rooms
                       where (r.RoomID == roomID) && (r.Capacity >= capacity)
                       select r).Count();
                var roomsCapacity = from c in systemDB.Rooms
                                    where (c.Capacity >= capacity)
                                    select c.RoomID;
                
                for (int k = 0; k < 5; k++)
                {
                    html += "<tr><th>" + days[k] + "</th>";
                    day = days[k];
                    for (int l = 0; l < 9; l++)
                    {
                        period = periods[l];
                        var rooms = from r in systemDB.Requests
                                     where (r.AcceptedRoom == roomID) && (r.Day == day) && (r.StartTime == period) 
                                     && (r.Semester == semester) && (r.Status == "Accepted") 
                                     && (roomsCapacity.Contains(r.AcceptedRoom))
                                     select r.RequestId;
                        
                        available = (rooms.Intersect(roomsWeeks)).Count();

                        available = max - available;
                        if (available == 0) { htmlClass = "redAv"; }
                        else if (available < (max / 2)) { htmlClass = "yellowAv"; }
                        else { htmlClass = "greenAv"; }
                        html += "<td class='"+htmlClass+"'>" + available + "/" + max + "</td>";
                    }
                    html += "</tr>";
                }
                html += "</table>";
            }
            else if (buildingName != "No Preference" && buildingName != "")
            {
                var buildingID = (from b in systemDB.Buildings
                                  where b.BuildingName == buildingName
                                  select b.BuildingID).Single();
                var roomsCapacity = from c in systemDB.Rooms
                                    where (c.Capacity >= capacity) && (c.BuildingID == buildingID)
                                    select c.RoomID;
                max = (from r in systemDB.Rooms
                       where (r.BuildingID == buildingID) && (r.Capacity >= capacity)
                       select r).Count();
                for (int k = 0; k < 5; k++)
                {
                    html += "<tr><th>" + days[k] + "</th>";
                    day = days[k];
                    for (int l = 0; l < 9; l++)
                    {
                        period = periods[l];
                        var buildings = from r in systemDB.Requests
                                     where (r.Building == buildingID) && (r.Day == day) && (r.StartTime == period)
                                     && (r.Semester == semester) && (r.Status == "Accepted")
                                     && (roomsCapacity.Contains(r.AcceptedRoom))
                                     select r.RequestId;
                        var buildingWeeks = from w in systemDB.RequestWeeks
                                         where (w.Week == week)
                                         select w.RequestId;
                        available = (buildings.Intersect(buildingWeeks)).Count();
                        available = max - available;
                        if (available == 0) { htmlClass = "redAv"; }
                        else if (available < (max / 2)) { htmlClass = "yellowAv"; }
                        else { htmlClass = "greenAv"; }
                        html += "<td class='" + htmlClass + "'>" + available + "/" + max + "</td>";
                    }
                    html += "</tr>";
                }
                html += "</table>";
            }
            else if (parkName != "No Preference" && parkName != "")
            {
                var parkID = (from p in systemDB.Parks
                              where p.ParkName == parkName
                              select p.ParkID).Single();
                var buildings = from b in systemDB.Buildings
                                where b.ParkID == parkID
                                select b.BuildingID;
                var roomsCapacity = from c in systemDB.Rooms
                                    where (c.Capacity >= capacity) && (buildings.Contains(c.BuildingID))
                                    select c.RoomID;
                foreach (int id in buildings)
                {
                    max += (from r in systemDB.Rooms
                            where (r.BuildingID == id) && (r.Capacity >= capacity)
                            select r).Count();
                }
                for (int k = 0; k < 5; k++)
                {
                    html += "<tr><th>" + days[k] + "</th>";
                    day = days[k];
                    for (int l = 0; l < 9; l++)
                    {
                        period = periods[l];
                        var parks = from r in systemDB.Requests
                                     where (r.Park == parkID) && (r.Day == day) && (r.StartTime == period)
                                     && (r.Semester == semester) && (r.Status == "Accepted")
                                     && (roomsCapacity.Contains(r.AcceptedRoom))
                                     select r.RequestId;
                        var parksWeeks = from w in systemDB.RequestWeeks
                                            where (w.Week == week)
                                            select w.RequestId;
                        available = (parks.Intersect(parksWeeks)).Count();
                        available = max - available;
                        if (available == 0) { htmlClass = "redAv"; }
                        else if (available < (max / 2)) { htmlClass = "yellowAv"; }
                        else { htmlClass = "greenAv"; }
                        html += "<td class='" + htmlClass + "'>" + available + "/" + max + "</td>";
                    }
                    html += "</tr>";
                }
                html += "</table>";
            }
            else
            {
                var parkIDs = from p in systemDB.Parks
                              select p.ParkID;
                var roomsCapacity = from c in systemDB.Rooms
                                    where (c.Capacity >= capacity)
                                    select c.RoomID;
                foreach (int id in parkIDs)
                {
                    var buildIDs = from b in systemDB.Buildings
                                   where b.ParkID == id
                                   select b.BuildingID;
                    foreach (int bid in buildIDs)
                    {
                        max += (from r in systemDB.Rooms
                                where (r.BuildingID == bid) && (r.Capacity >= capacity)
                                select r).Count();
                    }
                }
                for (int k = 0; k < 5; k++)
                {
                    html += "<tr><th>" + days[k] + "</th>";
                    day = days[k];
                    for (int l = 0; l < 9; l++)
                    {
                        period = periods[l];
                        var allReqs = from r in systemDB.Requests
                                     where (r.Day == day) && (r.StartTime == period)
                                     && (r.Semester == semester) && (r.Status == "Accepted")
                                     && (roomsCapacity.Contains(r.AcceptedRoom))
                                     select r.RequestId;
                        var weeks = from w in systemDB.RequestWeeks
                                            where (w.Week == week)
                                            select w.RequestId;
                        available = (allReqs.Intersect(weeks)).Count();
                        available = max - available;
                        if (available == 0) { htmlClass = "redAv"; }
                        else if (available < (max / 2)) { htmlClass = "yellowAv"; }
                        else { htmlClass = "greenAv"; }
                        html += "<td class='" + htmlClass + "'>" + available + "/" + max + "</td>";
                    }
                    html += "</tr>";
                }
                html += "</table>";
            }

            return html;
        }

    }
}
