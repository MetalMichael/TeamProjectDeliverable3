using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimetableSystem.Models;

namespace TimetableSystem.Helpers
{
    public class AvailabilityHelper
    {
        public static SelectList ParkSort(int park)
        {
            TimetableSystemEntities systemDB = new TimetableSystemEntities();

            var buildQry = from b in systemDB.Buildings
                           where b.ParkID == park
                           orderby b.BuildingName
                           select b.BuildingName;

            SelectList buildings = new SelectList(buildQry);
            return buildings;
        }
    }
}
