using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TimetableSystem.Models
{
    public class Request
    {
        [Key]
        [ScaffoldColumn(false)]
        public int RequestId { get; set; }

        [Required(ErrorMessage = "Total Students is required")]
        [DisplayName("Total Students")]
        public int StudentsTotal { get; set; }

        [DisplayName("Special Requirements")]
        public string SpecialRequest { get; set; }

        [ScaffoldColumn(false)]
        public string Department { get; set; }

        [Required(ErrorMessage = "Module is required")]
        [DisplayName("Module")]
        [ForeignKey("Module")]
        public int ModuleId { get; set; }

        public virtual Module Module { get; set; }

        public bool Priority { get; set; }

        [ScaffoldColumn(false)]
        public int Round { get; set; }

        [ScaffoldColumn(false)]
        public int Semester { get; set; }

        [Required(ErrorMessage = "Day is required")]
        public string Day { get; set; }

        [DisplayName("Time")]
        public int StartTime { get; set; }

        [DisplayName("Length")]
        public int Length { get; set; }

        [DisplayName("Room Type")]
        public string RoomType { get; set; }

        [DisplayName("Building")]
        public int BuildingId { get; set; }
        public virtual Building Building { get; set; }

        public int ParkId { get; set; }
        public virtual Park Park { get; set; }

        [ScaffoldColumn(false)]
        public int AcceptedRoom { get; set; }

        [ScaffoldColumn(false)]
        public string Status { get; set; }

        [ScaffoldColumn(false)]
        public List<int> Weeks { get; set; }
        public virtual List<RequestWeek> RequestWeeks { get; set; }

        public List<string> Rooms { get; set; }
        public virtual List<RequestRoom> RequestRooms { get; set; }

        //Features
        [DisplayName("Data Projector")]
        public bool Projector { get; set; }

        [DisplayName("Second Projector")]
        public bool Projector2 { get; set; }

        public bool OHP { get; set; }

        public bool Chalkboard { get; set; }

        [DisplayName("Large Board Area")]
        public bool BoardArea { get; set; }

        [DisplayName("Wheelchair Access")]
        public bool Wheelchair { get; set; }

        [DisplayName("Induction Loop")]
        public bool Induction { get; set; }


    }
}
