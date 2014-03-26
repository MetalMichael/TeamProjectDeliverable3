using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    public class RoomModel
    {
        [Key]
        public int RoomID { get; set; }
        public string RoomCode { get; set; }
        public int Capacity { get; set; }
        [ForeignKey("Building")]
        public int BuildingID { get; set; }
    }
}

