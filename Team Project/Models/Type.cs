﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    public class Type
    {
        [Key]
        public int RoomTypeID { get; set; }
        public string RoomType { get; set; }
    }
}