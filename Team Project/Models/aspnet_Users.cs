using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TimetableSystem.Models
{
    public class aspnet_Users
    {
        [Key]
        //[Required(ErrorMessage = "Please enter a module code.")]
        // triggers IEnumerable error
        public string ApplicationId { get; set; }
        //[Required(ErrorMessage = "Please enter a module title.")]
        // same story 
        public string UserId { get; set; }

        public string UserName { get; set; }
        public string MobileAlias { get; set; }
        public bool isAnonymous { get; set; }
        public string LastActivityDate { get; set; }
    }
}
