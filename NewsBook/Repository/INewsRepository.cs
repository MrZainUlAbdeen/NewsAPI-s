using NewsBook.Models;
using NewsBook.Models.Paging;

namespace NewsBook.Repository
{
    public interface INewsRepository : IBaseRepository<News>
    {
        Task<News> Insert(string title, string description);
        Task<News> Update(News news);
        Task<News> Delete(Guid id);
        Task<List<News>> GetAll();
        Task<PagedList<News>> GetAll(PagingParameters pagingParameters);
        Task<News?> GetById(Guid id);
        Task<List<News>> GetFavouriteNews();
        Task<PagedList<News>> GetFavouriteNews(PagingParameters pagingParameters);
        Task<News> Update(Guid id, string title, string description);
    }
}
