using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    public class BPRViewModel
    {
        public ParkModel Park { get ; set; }
        public BuildingModel Building { get ; set; }
        public RoomModel Room { get; set; }
    }
}

