using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    [Table("RoomFacilities")]
    public class RoomFacility
    {
        [Key, Column(Order = 0)]
        public int RoomID { get; set; }
        public virtual Room Room { get; set; }

        [Key, Column(Order = 1)]
        public int FacilityID { get; set; }
        public virtual Facility Facility { get; set; }
    }
}