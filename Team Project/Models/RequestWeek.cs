using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    public class RequestWeek
    {
        [Key, Column(Order = 0)]
        public int Week { get; set; }
        [Key, Column(Order = 1)]
        public int RequestId { get; set; }

        public virtual Request Request { get; set; }

        public RequestWeek(int week)
        {
            this.Week = week;
        }
    }
}
