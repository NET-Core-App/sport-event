using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SportEvent.Models
{
    public class Organizer
    {
        public int id { get; set; }

        [Required(ErrorMessage = "The organizer name field is required.")]
        [StringLength(100, ErrorMessage = "The organizer name must not exceed 100 characters.")]
        public string organizerName { get; set; }

        [Required(ErrorMessage = "The image location field is required.")]
        [StringLength(255, ErrorMessage = "The image location must not exceed 255 characters.")]
        public string imageLocation { get; set; }
    }
}
