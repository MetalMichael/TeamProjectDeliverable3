using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    public class Request
    {
        /*[Key]
        public int ID { get; set; }
        public Module module { get; set; }
        public bool priority { get; set; }
        public int day { get; set; }
        public int period { get; set; }
        public int length { get; set; }
        public int roomTotal { get; set; }
        public int studentTotal { get; set; }
        public string specialRequirements { get; set; }
        public User createdBy { get; set; }
        public DateTime created { get; set; }
        public Department department { get; set; }
        public Round round { get; set; }
        public string weeks { get; set; }
        public string features { get; set; }
        public string rooms { get; set; }
        public int status { get; set; }*/
        [Key]
        public int RequestID { get; set; }
        public int StudentsRequested { get; set; }
        public int SpecialRequest { get; set; }
        public string TimeCreated { get; set; }
        public int DepartmentUserID { get; set; }
        public int ModuleID { get; set; }
    }
}