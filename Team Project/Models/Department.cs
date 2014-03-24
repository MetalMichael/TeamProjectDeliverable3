using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    public class Department
    {
        [Key]
        public string code { get; set; }
        public string departmentCode { get; set; }
        public string departmentName { get; set; }
    }
}
