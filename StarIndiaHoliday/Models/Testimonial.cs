using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StarIndiaHoliday.Models
{
    public class Testimonial
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter user Name")]
        public string User_Name { get; set; }


        [Required(ErrorMessage = "Enter user post")]
        public string Post_Name { get; set; }

        [Required(ErrorMessage = "Enter description")]
        public string Description { get; set; }

        public string User_Image { get; set; }
    }
}