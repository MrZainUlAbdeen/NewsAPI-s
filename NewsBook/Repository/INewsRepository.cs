using NewsBook.Core;
using NewsBook.Models;
using NewsBook.Models.Paging;

namespace NewsBook.Repository
{
    public interface INewsRepository : IBaseResponse<News>
    {
        Task<News> Insert(string title, string description);
        Task<News> Update(News news);
        Task<News> Delete(Guid id);
        Task<List<News>> GetAll(string orderBy, bool isAscending);
        Task<PagedList<News>> GetAll(PagingParameters pagingParameters, string orderBy, bool isAscending);
        Task<News?> GetById(Guid id);
        Task<List<News>> GetFavouriteNews(string orderBy, bool isAscending);
        Task<PagedList<News>> GetFavouriteNews(PagingParameters pagingParameters, string orderBy, bool isAscending);
        Task<News> Update(Guid id, string title, string description);
    }
}
