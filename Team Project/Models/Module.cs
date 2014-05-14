using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TimetableSystem.Models
{
    public class Module
    {
        [Key]
        [Required(ErrorMessage = "Please enter a module code.")]
            // triggers IEnumerable error
        [DisplayName("Module code")]
        public string ModuleCode { get; set; }
        [Required(ErrorMessage = "Please enter a module title.")]
            // same story 
        [DisplayName("Module title")]
        public string ModuleTitle { get; set; }

        public string Department { get; set; }
    }
}
