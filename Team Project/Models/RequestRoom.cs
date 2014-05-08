using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    public class RequestRoom
    {
        [Key, Column(Order=1)]
        public int? RequestID { get; set; }
        public virtual Request Request { get; set; }

        [Key, Column(Order=2)]
        public int? RoomID { get; set; }
        public virtual Room Room { get; set; }
    }
}