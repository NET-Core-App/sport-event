using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SportEvent.Models
{
    public class LoginModel
    {
   
        public int id { get; set; }
        [Required(ErrorMessage = "The email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }

        [Required(ErrorMessage = "The password field is required.")]
        public string password { get; set; }
    }
}
