namespace NewsBook.Models
{
    public class News : TrackableBaseEntity
    {
        public Guid Id { get; set; }
<<<<<<< Updated upstream
        public string Title { get; set; }
        public string Description { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
=======
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
>>>>>>> Stashed changes
        
    }
}
