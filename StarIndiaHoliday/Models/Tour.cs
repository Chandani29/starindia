using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StarIndiaHoliday.Models
{
    public class Tour
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Enter categoryName")]
        public string categoryName { get; set; }
        [Required(ErrorMessage = "Enter destinationid")]
        public int destinationid { get; set; }
        [Required(ErrorMessage = "Enter tourName")]
        public string tourName { get; set; }
        [Required(ErrorMessage = "Enter shortDescription")]
        public string shortDescription { get; set; }
        public string facility { get; set; }
        [Required(ErrorMessage = "Enter price")]
        public int price { get; set; }
        
        public bool status { get; set; }
        public List<CheckModel> objCheck { get; set; }


        public string destination { get; set; }

        public string imageName { get; set; }
        [Required(ErrorMessage = "Upload Image")]
        [ValidateFile]
        public HttpPostedFileBase file1 { get; set; }

        public bool setAsHomePage { get; set; }
    }


    public class ValidateFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int MaxContentLength = 1024 * 1024 * 6; //6 MB
            string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".jpeg" };

            var file = value as HttpPostedFileBase;

            if (file == null)
                return false;
            else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
            {
                ErrorMessage = "Please upload Your Photo of type: " + string.Join(", ", AllowedFileExtensions);
                return false;
            }
            else if (file.ContentLength > MaxContentLength)
            {
                ErrorMessage = "Your Photo is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "MB";
                return false;
            }
            else
                return true;
        }
    }



    public class CheckModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }

    }


}