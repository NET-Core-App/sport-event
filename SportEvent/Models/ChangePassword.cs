using System.ComponentModel.DataAnnotations;

namespace SportEvent.Models
{
    public class ChangePassword 
    {

        [Required(ErrorMessage = "The password field is required.")]
        [Display(Name = "Old Password")]
        public string oldPassword { get; set; }

        [Required(ErrorMessage = "The password field is required.")]
        [Display(Name = "New Password")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*])(.{8,})$", ErrorMessage = "Password must contain at least one uppercase letter and one special character, and be at least 8 characters long.")]
        public string newPassword { get; set; }

        [Required(ErrorMessage = "The repeat password field is required.")]
        [Display(Name = "Repeat Password")]
        [Compare("newPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string repeatPassword { get; set; }
    }
}
