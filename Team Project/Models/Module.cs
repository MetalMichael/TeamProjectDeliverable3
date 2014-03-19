using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    public class Module
    {
        [Key]
        public string ModuleCode { get; set; }
        public string ModuleTitle { get; set; }
    }
}