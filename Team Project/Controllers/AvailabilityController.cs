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
            ViewBag.Title = "Availability";

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
                tempList.Add(r.RoomCode + "   (Cap: " + r.Capacity + ")");
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

            ViewBag.Department = User.Identity.Name;

            return View();
        }

        //*************PARK SELECTED METHOD**************
        public string parkSelected(string parkName, int roomType)
        {
            //Need to catch error here when changing back to '' on park dropdown
            string buildings = "";
            string rooms = "";
            string returnStr = "";
            var roomTypes = from r in systemDB.RoomTypes
                              select r;
            if (roomType != 0)
            {
                roomTypes = from r in roomTypes
                              where r.RoomTypeID == roomType
                              select r;
            }
            var roomTypeIDs = from r in roomTypes
                              select r.RoomID;
            var roomQry = from r in systemDB.Rooms
                          select r;

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
                if (roomType != 0)
                {
                    roomQry = from r in roomQry
                              where roomTypeIDs.Contains(r.RoomID)
                              select r;
                }
                foreach (Room r in roomQry)
                {
                    rooms += r.RoomCode + "   (Cap: " + r.Capacity + ");";
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
                    if (roomType != 0)
                    {
                        roomQry = from r in systemDB.Rooms
                                  where (r.BuildingID == b) && (roomTypeIDs.Contains(r.RoomID))
                                  select r;
                    }
                    else
                    {
                        roomQry = from r in systemDB.Rooms
                                  where (r.BuildingID == b)
                                  select r;
                    }
                    foreach (Room r in roomQry)
                    {
                        rooms += r.RoomCode + "   (Cap: " + r.Capacity + ");";
                    }
                }
            }
            buildings = buildings.Substring(0, buildings.Length - 1);
            rooms = rooms.Substring(0, rooms.Length - 1);
            returnStr = buildings + "!" + rooms;

            return returnStr;
        }


        //********BUILDING SELECTED METHOD ************
        public string buildingSelected(string buildingName, int roomType)
        {
            string rooms = "";

            var roomTypes = from r in systemDB.RoomTypes
                            select r;
            if (roomType != 0)
            {
                roomTypes = from r in roomTypes
                            where r.RoomTypeID == roomType
                            select r;
            }
            var roomTypeIDs = from r in roomTypes
                              select r.RoomID;
            var roomQry = from r in systemDB.Rooms
                          select r;

            if (buildingName == "")
            {
                roomQry = from r in roomQry
                            where roomTypeIDs.Contains(r.RoomID)
                            select r;

                foreach (Room r in roomQry)
                {
                    rooms += r.RoomCode + "   (Cap: " + r.Capacity + ");";
                }
            }
            else
            {
                var buildID = (from b in systemDB.Buildings
                               where b.BuildingName == buildingName
                               select b.BuildingID).First();
                if (roomType != 0)
                {
                    roomQry = from r in roomQry
                              where (r.BuildingID == buildID) && (roomTypeIDs.Contains(r.RoomID))
                              select r;
                }
                else
                {
                    roomQry = from r in roomQry
                              where r.BuildingID == buildID
                              select r;
                }
                foreach (Room r in roomQry)
                {
                    rooms += r.RoomCode + "   (Cap: " + r.Capacity.ToString() + ");";
                }
            }
            if (rooms.Length > 0)
            {
                rooms = rooms.Substring(0, rooms.Length - 1);
            }

            return rooms;
        }

        public string typeSelected(string filter, int roomType, string filterType)
        {
            string rooms = "";

            var roomDB = from r in systemDB.Rooms
                        select r;

            var roomTypes = from r in systemDB.RoomTypes
                              select r;
            var roomTypeIDs = from r in systemDB.RoomTypes
                              select r.RoomID;

            if (roomType != 0)
            {
                roomTypeIDs = from r in roomTypes
                              where (r.RoomTypeID == roomType)
                              select r.RoomID;
            }
            
            if (filterType == "room")
            {
                roomDB = from r in roomDB
                         where r.RoomCode == filter
                         select r;
            }
            else if (filterType == "building")
            {
                var buildingID = (from b in systemDB.Buildings
                                 where b.BuildingName == filter
                                 select b.BuildingID).Single();
                roomDB = from r in roomDB
                         where r.BuildingID == buildingID
                         select r;
            }
            else if (filterType == "park")
            {
                var parkID = (from p in systemDB.Parks
                              where p.ParkName == filter
                              select p.ParkID).Single();
                var buildQry = (from b in systemDB.Buildings
                                where b.ParkID == parkID
                                select b.BuildingID).Distinct();
                roomDB = from r in roomDB
                         where (buildQry.Contains(r.BuildingID))
                         select r;
            }

            roomDB = from r in roomDB
                     where (roomTypeIDs.Contains(r.RoomID))
                     select r;

            foreach (Room r in roomDB)
            {
                rooms += r.RoomCode + "   (Cap: " + r.Capacity.ToString() + ");";
            }
            if (rooms.Length > 0)
            {
                rooms = rooms.Substring(0, rooms.Length - 1);
            }

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
            string[] times = {"9:00-9:50 / Period 1", "10:00-10:50 / Period 2", "11:00-11:50 / Period 3", "12:00-12:50 / Period 4", 
                                 "13:00-13:50 / Period 5", "14:00-14:50 / Period 6", "15:00-15:50 / Period 7", "16:00-16:50 / Period 8", 
                                 "17:00-17:50 / Period 9"};
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

            string html = "<table class='availability'><thead><tr><th class='day'><p style='text-align: center'>-</p></th>";
            for (int i = 0; i < 9; i++)
            {
                html += "<th class='day'>" + times[i] + "</th>";
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
                        slot = "slot" + (k + 1) + (l + 1);
                        html += "<td onclick='listRooms(\"" + slot + "\")' id='" + slot + "' class='" + htmlClass + "'>"
                            + available + "/" + max + " Rooms Available</td>";
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
                                     where (r.BuildingId == buildingID) && (r.Day == day) && (r.StartTime == period)
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
                        slot = "slot" + (k + 1) + (l + 1);
                        html += "<td onclick='listRooms(\"" + slot + "\")' id='" + slot + "' class='" + htmlClass + "'>"
                            + available + "/" + max + " Rooms Available</td>";
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
                                     where (r.ParkId == parkID) && (r.Day == day) && (r.StartTime == period)
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
                        slot = "slot" + (k + 1) + (l + 1);
                        html += "<td onclick='listRooms(\"" + slot + "\")' id='" + slot + "' class='" + htmlClass + "'>"
                            + available + "/" + max + " Rooms Available</td>";
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
                        html += "<td onclick='listRooms(\""+slot+"\")' id='" + slot + "' class='" + htmlClass + "'>"
                            + available + "/" + max + " Rooms Available</td>";
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
        public int getRoomID(string roomCode)
        {
            int room = (from r in systemDB.Rooms
                       where r.RoomCode == roomCode
                       select r.RoomID).Single();
            return room;
        }

    }
}
