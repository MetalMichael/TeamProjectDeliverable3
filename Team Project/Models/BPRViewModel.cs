using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    public class BPRViewModel
    {
        public Park xPark { get ; set; }
        public Building xBuilding { get ; set; }
        public Room xRoom { get; set; }
    }
}

