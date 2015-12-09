using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MemberRMS.Models
{
    public class Register
    {
        [Display(Name = "Username*")]
        [Required(ErrorMessage = "You must enter a username or E-mail")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Sorry the username or password is invalid")]
        public string Username { get; set; }

        [StringLength(100, ErrorMessage = "The password must contain at least {2} characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password*")]
        [Required(ErrorMessage = "You must enter your password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password*")]
        [Compare("Password", ErrorMessage = "Password and confirmation don't match")]

        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "You must enter a first name")]
        [Display(Name = "First name*")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You must enter a last name")]
        [Display(Name = "Last name*")]
        public string LastName { get; set; }
        [Display(Name = "Date of Birth")]
        public string Birthday { get; set; }
        [Display(Name = "Phone")]
        public string Telephone { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }


    }
}