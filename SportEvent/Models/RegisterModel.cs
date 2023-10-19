using System.ComponentModel.DataAnnotations;

namespace SportEvent.Models
{
    public class RegisterModel
    {
        /*  [Required(ErrorMessage = "The first name field is required.")]
          [Display(Name = "First Name")]
          public string firstName { get; set; }

          [Required(ErrorMessage = "The last name field is required.")]
          [Display(Name = "Last Name")]
          public string lastName { get; set; }

          [Required(ErrorMessage = "The email field is required.")]
          [Display(Name = "Email")]
          [EmailAddress(ErrorMessage = "Invalid Email Address")]
          public string email { get; set; }

          [Required(ErrorMessage = "The password field is required.")]
          [Display(Name = "Password")]
          [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*])(.{8,})$", ErrorMessage = "Password must contain at least one uppercase letter and one special character, and be at least 6 characters long.")]
          public string password { get; set; }

          [Required(ErrorMessage = "The repeat password field is required.")]
          [Display(Name = "Repeat Password")]
          [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
          [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*])(.{8,})$", ErrorMessage = "Password must contain at least one uppercase letter and one special character, and be at least 6 characters long.")]
          public string repeatPassword { get; set; }*/

        [Required(ErrorMessage = "The first name field is required.")]
        [Display(Name = "First Name")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "The last name field is required.")]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [Required(ErrorMessage = "The email field is required.")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }

        [Required(ErrorMessage = "The password field is required.")]
        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*])(.{8,})$", ErrorMessage = "Password must contain at least one uppercase letter and one special character, and be at least 8 characters long.")]
        public string password { get; set; }

        [Required(ErrorMessage = "The repeat password field is required.")]
        [Display(Name = "Repeat Password")]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        public string repeatPassword { get; set; }
    }
}
    