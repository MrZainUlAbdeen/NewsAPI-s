namespace NewsBook.Models
{
    public class Picture : TrackableBaseEntity
    {
        public string Profile { get; set; } = string.Empty;
        
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
