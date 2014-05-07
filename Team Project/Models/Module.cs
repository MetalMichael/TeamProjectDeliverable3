using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    public class Module
    {
        [Key]
        public int ModuleID { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleTitle { get; set; }
        public string Department { get; set; }
    }
}
