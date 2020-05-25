using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StarIndiaHoliday.Models
{
    public class touristFeedback
    {

        public int id { get; set; }
        [Required(ErrorMessage = "Enter name")]
        public string name { get; set; }
        [Required(ErrorMessage = "Enter designatin")] 
        public string designatin { get; set; } 
        [Required(ErrorMessage = "Enter imageName")]
        public string imageName { get; set; }
        [Required(ErrorMessage = "Enter shortDescription")]
        public string shortDescription { get; set; }
        [RegularExpression(@"^\(?([5-9]{1})\)?([0-9]{2})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered mobile format is not valid.")]
        [Required(ErrorMessage = "Enter mobile")]
        public string mobileNumber { get; set; }
        public bool status { get; set; }
        [Required(ErrorMessage = "Upload Image")]
        [ValidateFile]
        public HttpPostedFileBase file1 { get; set; }

        public List<touristFeedback> touristFeedbacklist { get; set; } 

    }
}