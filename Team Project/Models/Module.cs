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

        //[Required(ErrorMessage = "Please enter a module code.")]
            // triggers IEnumerable error
        public string ModuleCode { get; set; }
        //[Required(ErrorMessage = "Please enter a module title.")]
            // same story 
        public string ModuleTitle { get; set; }

        public string Department { get; set; }
    }
}
