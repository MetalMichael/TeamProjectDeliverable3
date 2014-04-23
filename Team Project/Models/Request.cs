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
        public int ModuleId { get; set; }

        public byte Priority { get; set; }

        [ScaffoldColumn(false)]
        public int Round { get; set; }

        [ScaffoldColumn(false)]
        public int Semester { get; set; }

        [Required(ErrorMessage = "Day is required")]
        public string Day { get; set; }

        public int StartTime { get; set; }

        public int Length { get; set; }

        public string RoomType { get; set; }

        public int Building { get; set; }

        public int Park { get; set; }

        [ScaffoldColumn(false)]
        public int AcceptedRoom { get; set; }

        [ScaffoldColumn(false)]
        public string Status { get; set; }

        public virtual List<RequestWeek> RequestWeeks { get; set; }

        [ScaffoldColumn(false)]
        public List<int> Weeks { get; set; }
    }
}
