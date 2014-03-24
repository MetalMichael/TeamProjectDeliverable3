using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string Username { get; set; }

    }
}