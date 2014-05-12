using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    public class RoomType
    {
        [Key]
        public int ID { get; set; }

        public int RoomID { get; set; }
        public virtual Room Room { get; set; }
        
        public int RoomTypeID { get; set; }
        public virtual Type Type { get; set; }
    }
}