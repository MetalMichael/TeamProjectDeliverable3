using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    [Table("Facilities")]
    public class Facility
    {
        [Key]
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
    }
}
