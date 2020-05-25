using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StarIndiaHoliday.Models
{
    public class Destination
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Enter categoryName")]
        public string categoryName { get; set; }
        [Required(ErrorMessage = "Enter destination")]
        public string destination { get; set; }
        public bool status { get; set; }
    }
}