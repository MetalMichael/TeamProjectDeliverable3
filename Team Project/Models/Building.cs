using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    public class Building
    {
        [Key]
        public int BuildingID { get; set; }
        public string BuildingName { get; set; }
        //[ForeignKey("Park")]
        public int ParkID { get; set; }
    }
}

