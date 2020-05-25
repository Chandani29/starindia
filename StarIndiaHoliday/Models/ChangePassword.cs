using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StarIndiaHoliday.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Old Password Is Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "new Password Is Required")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "conpassword Password Is Required")]
        [Compare("newpassword", ErrorMessage = "The Confirm Password you've entered do not match with New Password")]
        public string ConfirmPassword { get; set; }

    }
}