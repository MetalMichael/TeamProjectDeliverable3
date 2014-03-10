using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team_Project_Deliverable_3.Models
{
    public class Round
    {
        private int ID { get; set; }
        public int roundID { get; set; }
        public int semester { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public bool current { get; set; }
    }
}