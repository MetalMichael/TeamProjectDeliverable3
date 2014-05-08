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

            ViewBag.Type = new SelectList(new string[] { "Lecture", "Seminar", "Lab" });

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
        public string getAvailability(string parkName, string buildingName, string roomCode, int semester, string week, 
            int capacity, string roomType)
        {
            int max = 0; //max number of rooms
            int available = 0; //number of available rooms
            string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            string day, htmlClass, slot;
            string[] times = {"9:00-9:50", "10:00-10:50", "11:00-11:50", "12:00-12:50", "13:00-13:50", "14:00-14:50", "15:00-15:50",
                        "16:00-16:50", "17:00-17:50"};
            int[] periods = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int period, roomTypeID;
            

            if (roomType == "Lab")
                roomTypeID = 3;
            else if (roomType == "Seminar")
                roomTypeID = 2;
            else if (roomType == "Lecture")
                roomTypeID = 1;
            else
                roomTypeID = 0;

            var roomTypeIDs = from r in systemDB.RoomTypes
                                where r.RoomTypeID == roomTypeID
                                select r.RoomID;

            string html = "<table class='availability'><thead><tr><th><p style='text-align: center'>-</p></th>";
            for (int i = 0; i < 9; i++)
            {
                html += "<th>" + times[i] + "</th>";
            }
            html += "</tr></thead>";

            string[] temp = week.Split(';');
            List<int> weeks = new List<int>();
            for (int i = 0; i < temp.Length; i++)
            {
                weeks.Add(Convert.ToInt32(temp[i]));
            }

            var roomsWeeks = (from w in systemDB.RequestWeeks
                             where (weeks.Contains(w.Week))
                             select w.RequestId).Distinct();

            if (roomCode != "No" && roomCode != "")
            {
                //finding roomID from roomCode given
                var roomID = (from r in systemDB.Rooms
                              where r.RoomCode == roomCode
                              select r.RoomID).Single();
                //finding all rooms with large enough capacity.
                var roomsCapacity = from c in systemDB.Rooms
                                    where (c.Capacity >= capacity)
                                    select c.RoomID;

                if (roomTypeID != 0) //if room type is given, find max rooms inc room type
                {
                    max = (from r in systemDB.Rooms
                           where (r.RoomID == roomID) && (r.Capacity >= capacity) && (roomTypeIDs.Contains(r.RoomID))
                           select r).Count();
                }
                else // if room type not given, find max rooms from room ID and capacity.
                {
                    max = (from r in systemDB.Rooms
                           where (r.RoomID == roomID) && (r.Capacity >= capacity)
                           select r).Count();
                }
                
                for (int k = 0; k < 5; k++)
                {
                    html += "<tr class='bottom'><th class='day'>" + days[k] + "</th>";
                    day = days[k];
                    for (int l = 0; l < 9; l++)
                    {
                        period = periods[l];
                        var rooms = from r in systemDB.Requests
                                    where (r.AcceptedRoom == roomID) && (r.Day == day) && (r.StartTime == period)
                                    && (r.Semester == semester) && (r.Status == "Accepted") && (roomsWeeks.Contains(r.RequestId))
                                    && (roomsCapacity.Contains(r.AcceptedRoom))
                                    select r;
                        if (roomTypeID != 0)
                        {
                            rooms = from r in rooms
                                    where (roomTypeIDs.Contains(r.AcceptedRoom))
                                    select r;
                        }

                        available = max - rooms.Count();
                        if (available == 0) { htmlClass = "unavailable"; }
                        else if (available < (max / 2)) { htmlClass = "some"; }
                        else { htmlClass = "available"; }
                        html += "<td id='slot" + (k + 1) + (l + 1) + "' class='" + htmlClass + "'>" + available + "/" + max + " Rooms Available</td>";
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
                if (roomTypeID != 0) //if room type is given, find max rooms inc room type
                {
                    max = (from r in systemDB.Rooms
                           where (r.BuildingID == buildingID) && (r.Capacity >= capacity) && (roomTypeIDs.Contains(r.RoomID))
                           select r).Count();
                }
                else // if room type not given, find max rooms from building ID and capacity.
                {
                    max = (from r in systemDB.Rooms
                           where (r.BuildingID == buildingID) && (r.Capacity >= capacity)
                           select r).Count();
                }
                for (int k = 0; k < 5; k++)
                {
                    html += "<tr class='bottom'><th class='day'>" + days[k] + "</th>";
                    day = days[k];
                    for (int l = 0; l < 9; l++)
                    {
                        period = periods[l];
                        var rooms = from r in systemDB.Requests
                                     where (r.Building == buildingID) && (r.Day == day) && (r.StartTime == period)
                                     && (r.Semester == semester) && (r.Status == "Accepted") && (roomsWeeks.Contains(r.RequestId))
                                     && (roomsCapacity.Contains(r.AcceptedRoom))
                                     select r;

                        if (roomTypeID != 0)
                        {
                            rooms = from r in rooms
                                    where (roomTypeIDs.Contains(r.AcceptedRoom))
                                    select r;
                        }

                        available = max - rooms.Count();
                        if (available == 0) { htmlClass = "unavailable"; }
                        else if (available < (max / 2)) { htmlClass = "some"; }
                        else { htmlClass = "available"; }
                        html += "<td id='slot" + (k + 1) + (l + 1) + "' class='" + htmlClass + "'>" + available + "/" + max + " Rooms Available</td>";
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
                    if (roomTypeID != 0) //if room type is given, find max rooms inc room type
                    {
                        max += (from r in systemDB.Rooms
                               where (r.BuildingID == id) && (r.Capacity >= capacity) && (roomTypeIDs.Contains(r.RoomID))
                               select r).Count();
                    }
                    else // if room type not given, find max rooms from building ID and capacity.
                    {
                        max += (from r in systemDB.Rooms
                               where (r.BuildingID == id) && (r.Capacity >= capacity)
                               select r).Count();
                    }
                }
                for (int k = 0; k < 5; k++)
                {
                    html += "<tr class='bottom'><th class='day'>" + days[k] + "</th>";
                    day = days[k];
                    for (int l = 0; l < 9; l++)
                    {
                        period = periods[l];
                        var rooms = from r in systemDB.Requests
                                     where (r.Park == parkID) && (r.Day == day) && (r.StartTime == period)
                                     && (r.Semester == semester) && (r.Status == "Accepted") && (roomsWeeks.Contains(r.RequestId))
                                     && (roomsCapacity.Contains(r.AcceptedRoom))
                                     select r;
                        if (roomTypeID != 0)
                        {
                            rooms = from r in rooms
                                    where (roomTypeIDs.Contains(r.AcceptedRoom))
                                    select r;
                        }

                        available = max - rooms.Count();
                        if (available == 0) { htmlClass = "unavailable"; }
                        else if (available < (max / 2)) { htmlClass = "some"; }
                        else { htmlClass = "available"; }
                        html += "<td id='slot" + (k + 1) + (l + 1) + "' class='" + htmlClass + "'>" + available + "/" + max + " Rooms Available</td>";
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
                        if (roomTypeID != 0) //if room type is given, find max rooms inc room type
                        {
                            max += (from r in systemDB.Rooms
                                    where (r.BuildingID == bid) && (r.Capacity >= capacity) && (roomTypeIDs.Contains(r.RoomID))
                                    select r).Count();
                        }
                        else // if room type not given, find max rooms from building ID and capacity.
                        {
                            max += (from r in systemDB.Rooms
                                    where (r.BuildingID == bid) && (r.Capacity >= capacity)
                                    select r).Count();
                        }
                    }
                }
                for (int k = 0; k < 5; k++)
                {
                    html += "<tr class='bottom'><th class='day'>" + days[k] + "</th>";
                    day = days[k];
                    for (int l = 0; l < 9; l++)
                    {
                        period = periods[l];
                        var rooms = from r in systemDB.Requests
                                     where (r.Day == day) && (r.StartTime == period)
                                     && (r.Semester == semester) && (r.Status == "Accepted") && (roomsWeeks.Contains(r.RequestId))
                                     && (roomsCapacity.Contains(r.AcceptedRoom))
                                     select r;

                        if (roomTypeID != 0)
                        {
                            rooms = from r in rooms
                                    where (roomTypeIDs.Contains(r.AcceptedRoom))
                                    select r;
                        }


                        available = max - rooms.Count();
                        if (available == 0) { htmlClass = "unavailable"; }
                        else if (available < (max / 2)) { htmlClass = "some"; }
                        else { htmlClass = "available"; }
                        slot = "slot" + (k+1) + (l+1);
                        html += "<td id='" + slot + "' class='" + htmlClass + "'><p class='slot'>"
                            + available + "/" + max + " Rooms Available</p></td>";
                    }
                    html += "</tr>";
                }
                html += "</table>";
            }
            //return 0;
            return html;
        }

        public string checkSlot(string parkName, string buildingName, string roomCode, int semester, string week,
            int capacity, string roomType, string slot)
        {
            string x = "";
            string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            int period = Convert.ToInt32(slot.Substring(5, 1));
            int dayNum = Convert.ToInt32(slot.Substring(4, 1));
            
            //return slot;
            string day = days[dayNum-1];
            int roomTypeID;

            //setting room type to equivalent ID
            if (roomType == "Lab")
                roomTypeID = 3;
            else if (roomType == "Seminar")
                roomTypeID = 2;
            else if (roomType == "Lecture")
                roomTypeID = 1;
            else
                roomTypeID = 0;

            var roomTypeIDs = from r in systemDB.RoomTypes
                              where r.RoomTypeID == roomTypeID
                              select r.RoomID;

            //checking for requests in week span 
            string[] temp = week.Split(';');
            List<int> weeks = new List<int>();
            for (int i = 0; i < temp.Length; i++)
            {
                weeks.Add(Convert.ToInt32(temp[i]));
            }
            var roomsWeeks = (from w in systemDB.RequestWeeks
                              where (weeks.Contains(w.Week))
                              select w.RequestId).Distinct();

            //All rooms with enough capacity
            var rooms = from r in systemDB.Rooms
                        where r.Capacity >= capacity
                        select r;
            //If room type is set, filtering rooms variable for only that room type
            if (roomTypeID != 0)
            {
                rooms = from r in rooms
                        where (roomTypeIDs.Contains(r.RoomID))
                        select r;
            }
            //All reqs with correct sem, period, day and status
            var requests = from r in systemDB.Requests
                           where (r.Semester == semester) && (r.StartTime == period) && (r.Day == day) && (r.Status == "Accepted")
                           && (roomsWeeks.Contains(r.RequestId))
                           select r;

            if (roomCode != "No" && roomCode != "")
            {
                var roomID = (from r in systemDB.Rooms
                              where r.RoomCode == roomCode
                              select r.RoomID).Single();

                //available rooms with this filter
                rooms = from r in rooms
                        where r.RoomCode == roomCode
                        select r;

                requests = from r in requests
                           where (r.AcceptedRoom == roomID)
                           select r;
                var reqRoomIDs = from r in requests
                                 select r.AcceptedRoom;
                var reqRoomCodes = from r in systemDB.Rooms
                                   where (reqRoomIDs.Contains(r.RoomID))
                                   select r.RoomCode;
                var roomCodes = from r in rooms
                            select r.RoomCode;
                List<string> xRooms = roomCodes.ToList();
                List<string> occupiedRoom = reqRoomCodes.ToList();

                var availableRooms = xRooms.Except(occupiedRoom);

                foreach (string s in availableRooms)
                {
                    x += s + ";";
                }
            }
            else if (buildingName != "No Preference" && buildingName != "")
            {
                var buildingID = (from b in systemDB.Buildings
                                  where b.BuildingName == buildingName
                                  select b.BuildingID).Single();
                var roomIDs = from r in systemDB.Rooms
                              where r.BuildingID == buildingID
                              select r.RoomID;

                rooms = from r in rooms
                        where r.BuildingID == buildingID
                        select r;

                requests = from r in requests
                           where (roomIDs.Contains(r.AcceptedRoom))
                           select r;
                var reqRoomIDs = from r in requests
                                 select r.AcceptedRoom;
                var reqRoomCodes = from r in systemDB.Rooms
                                   where (reqRoomIDs.Contains(r.RoomID))
                                   select r.RoomCode;

                var roomCodes = from r in rooms
                                select r.RoomCode;

                List<string> xRooms = roomCodes.ToList();
                List<string> occupiedRoom = reqRoomCodes.ToList();

                var availableRooms = xRooms.Except(occupiedRoom);

                foreach (string s in availableRooms)
                {
                    x += s + ";";
                }
            }
            else if (parkName != "No Preference" && parkName != "")
            {
                var parkID = (from p in systemDB.Parks
                              where p.ParkName == parkName
                              select p.ParkID).Single();
                var buildings = from b in systemDB.Buildings
                                where b.ParkID == parkID
                                select b.BuildingID;
                var roomIDs = from r in systemDB.Rooms
                              where (buildings.Contains(r.BuildingID))
                              select r.RoomID;

                rooms = from r in rooms
                        where (buildings.Contains(r.BuildingID))
                        select r;
                requests = from r in requests
                           where (roomIDs.Contains(r.AcceptedRoom))
                           select r;
                var reqRoomIDs = from r in requests
                                 select r.AcceptedRoom;
                var reqRoomCodes = from r in systemDB.Rooms
                                   where (reqRoomIDs.Contains(r.RoomID))
                                   select r.RoomCode;

                var roomCodes = from r in rooms
                                select r.RoomCode;

                List<string> xRooms = roomCodes.ToList();
                List<string> occupiedRoom = reqRoomCodes.ToList();

                var availableRooms = xRooms.Except(occupiedRoom);

                foreach (string s in availableRooms)
                {
                    x += s + ";";
                }
            }
            else
            {

                var reqRoomIDs = from r in requests
                                 select r.AcceptedRoom;
                var reqRoomCodes = from r in systemDB.Rooms
                                   where (reqRoomIDs.Contains(r.RoomID))
                                   select r.RoomCode;

                var roomCodes = from r in rooms
                                select r.RoomCode;

                List<string> xRooms = roomCodes.ToList();
                List<string> occupiedRoom = reqRoomCodes.ToList();

                var availableRooms = xRooms.Except(occupiedRoom);

                foreach (string s in availableRooms)
                {
                    x += s + ";";
                }

            }

            return x;
        }

    }
}
