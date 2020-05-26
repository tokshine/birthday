using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HappyBirthday.Models
{
    public class ProfileViewModel
    {
        public int Id { get; set; }

       
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "First name *")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name *")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "BirthDay/Month *")]
        public int? SelectedDay { get; set; }

        
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{5})$", ErrorMessage = "Entered phone format is not valid.")]
        [Display(Name = "Phone format (XXX-YYY-ZZZZZ)")]
        public string TelephoneNumber {get; set;}

        [Required]
        [Display(Name = "Month")]
        public int? SelectedMonth { get; set; }

      

        public IEnumerable<SelectListItem> Months { get; set; }
        public IEnumerable<SelectListItem> Days { get; set; }
        

    }

    public class Day
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Month
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class ProfileImage
    {
        public string ImageScr { get; set; }
        public bool ImageAvailable { get {

                return !string.IsNullOrEmpty(ImageScr);
            } }
        public string AlternateText { get; set; }
        public int Id { get; set; }
        [Display(Name ="Profile image")]
        public string LogoText { get; set; }
        
    }


    public class Logo
    {
        [Display(Name = "Upload image")]
        public string LogoText { get; set; }

        public string ImageScr { get; set; }
        public string AlternateText { get; set; }
        public bool ImageAvailable { get; set; }
        public int Id { get; set; }
        public System.Data.Linq.Binary ContentFile { get; set; }
        public string ContentType { get; set; }
        public string ContentName { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}