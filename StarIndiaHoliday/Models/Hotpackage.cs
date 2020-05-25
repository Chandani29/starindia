using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StarIndiaHoliday.Models
{
    public class Hotpackage
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Select Package Name")]
        public string PackageName { get; set; }
       
        public string Duration { get; set; }
      
        public string ShortDescription { get; set; }

        //[Required(ErrorMessage = "Enter Details ")]
        public string Details { get; set; }
        public string TermsConditions { get; set; }

        [Required(ErrorMessage = "Enter Package Cost ")]
        public string PackageCost{get; set;}
        public string photo1 { get; set; }
        public string photo2 { get; set; }
        public bool Status { get; set; }


        //[ValidateFile]
        public HttpPostedFileBase file1 { get; set; }
        //[ValidateFile]
        public HttpPostedFileBase file2 { get; set; }


    }
}