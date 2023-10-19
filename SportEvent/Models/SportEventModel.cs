namespace SportEvent.Models
{
    public class SportEventModel
    {
        public string id { get; set; }
        public string eventDate { get; set; }
        public string eventName { get; set; }
        public string eventType { get; set; }
        public Organizer organizer { get; set; }
    }
}
