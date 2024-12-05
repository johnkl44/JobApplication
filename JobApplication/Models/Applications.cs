using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace JobApplication.Models
{
    public class Applications
    {
        [Key]
        public int ApplicationID { get; set; }

        [Required(ErrorMessage = "Enter Your Name")]
        [Display(Name = "First Name")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "First Name should not be less than or equal to Two characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter Your Name")]
        [Display(Name = "Last Name")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Last Name should not be less than or equal to four characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter Your EmailID")]
        [Display(Name = "Email ID")]
        [RegularExpression(@"^[\w-\._\+%]+@(?:[\w-]+\.)+[\w]{2,6}$", ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must provide a Phone Number")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Enter Your Place")]
        [Display(Name = "Place")]
        [StringLength(20,MinimumLength = 3, ErrorMessage = "Place should not be less than or equal to two characters.")]
        public string Place { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime DOB {  get; set; }


        [Required(ErrorMessage = "Upload a photo")]
        [Display(Name = "Profile Photo")]
        public HttpPostedFileBase PhotoFile { get; set; }

        [Required(ErrorMessage = "Upload a file")]
        [Display(Name = "Resume")]
        public HttpPostedFileBase ResumeFile { get; set; }

        public byte[] Resume { get; set; }
        public byte[] Photo { get; set; }
        [Display(Name = "Profile Photo")]
        public string ImageBase64 { get; set; }
        [Display(Name = "Resume")]
        public string ResumeBase64 { get; set; }
    }
}