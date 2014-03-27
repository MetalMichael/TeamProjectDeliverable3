using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    public class Park
    {
        [Key]
        public int ParkID { get; set; }
        public string ParkName { get; set; }
    }
}