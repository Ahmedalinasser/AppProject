using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "New Password is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm new Password is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password Mach is not Match ")]
        public string ConfirmPassword { get; set;}

    }
}
