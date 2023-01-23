namespace NewsBook.Models
{
    public class FavouriteNews
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid NewsId { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
