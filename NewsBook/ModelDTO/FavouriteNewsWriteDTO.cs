namespace NewsBook.ModelDTO
{
    public class FavouriteNewsWriteDTO
    {
        public Guid NewsId { get; set; }
        public Guid UserId { get; set; }
        public bool IsFavourite { get; set; }
    }
}
