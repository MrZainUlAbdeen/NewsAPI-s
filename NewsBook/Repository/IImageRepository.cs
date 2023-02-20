namespace NewsBook.Repository
{
    public interface IImageRepository
    {
        Task<string> Add(string image);
    }
}
