using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }
        
        public string RoomCode { get; set; }
        
        public int Capacity { get; set; }
        
        public virtual Building Building { get; set; }

        public int BuildingID { get; set; }
    }
}

