using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class RegisterViewModel
    {
        

        [Required(ErrorMessage = "Frist name is required")]
        public string FName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string LName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password Mach is Not Match ")]
        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }


    }
}
