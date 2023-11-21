using System.ComponentModel.DataAnnotations;

namespace SportEvent.Models
{
    public class SportEventCreateModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The event date field is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime eventDate { get; set; }

        [Required(ErrorMessage = "The event type field is required.")]
        [StringLength(50, ErrorMessage = "The event type must not exceed 50 characters.")]
        public string eventType { get; set; }

        [Required(ErrorMessage = "The event name field is required.")]
        [StringLength(100, ErrorMessage = "The event name must not exceed 100 characters.")]
        public string eventName { get; set; }

        [Required(ErrorMessage = "The organizer ID field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid organizerId.")]
        public int organizerId { get; set; }
    }
}
