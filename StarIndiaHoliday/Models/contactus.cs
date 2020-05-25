using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StarIndiaHoliday.Models
{
    public class contactus
    {
        [Required(ErrorMessage = "Enter name")]
        public string name { get; set; }
        [RegularExpression(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Please enter a valid e-mail adress.")]
        [Required(ErrorMessage = "Enter email")]
        public string email { get; set; }
        [RegularExpression(@"^\(?([5-9]{1})\)?([0-9]{2})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered mobile format is not valid.")]
        [Required(ErrorMessage = "Enter mobile")]
        public string mobile { get; set; }
        [Required(ErrorMessage = "Enter message")]
        public string message { get; set; }
        [Required(ErrorMessage = "Enter captcha")]
        public string captcha { get; set; } 
        public string enquiryforTour { get; set; }
    }
}