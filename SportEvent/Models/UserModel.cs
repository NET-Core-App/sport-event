using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SportEvent.Models
{
    public class UserModel
    {
        [Display(Name = "Id")]
        public int id { get; set; }

        [Display(Name = "First Name")]
        public string firstName { get; set; }

        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }
    }
}
