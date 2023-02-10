namespace NewsBook.Response
{
    public class NewsResponse
    {
        public Guid UserId { get; set; }
        public Guid NewsId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
