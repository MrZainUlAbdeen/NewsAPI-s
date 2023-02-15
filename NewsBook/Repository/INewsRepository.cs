using NewsBook.Core;
using NewsBook.Models;
using NewsBook.Models.Paging;
using System.Linq.Expressions;

namespace NewsBook.Repository
{
    public interface INewsRepository : IBaseRepository<News>
    {
        Task<News> Insert(string title, string description);
        Task<News> Update(News news);
        Task<News> Delete(Guid id);
        Task<List<News>> GetAll(string orderBy, bool isAscending, Expression<Func<News, bool>>? filterBy, Guid userId);
        Task<PagedList<News>> GetAll(PagingParameters pagingParameters, string orderBy, bool isAscending, Expression<Func<News, bool>>? filterBy, Guid userId);
        Task<News?> GetById(Guid id);
        Task<List<News>> GetFavouriteNews(string orderBy, bool isAscending);
        Task<PagedList<News>> GetFavouriteNews(PagingParameters pagingParameters, string orderBy, bool isAscending);
        Task<News> Update(string title, string description);
    }
}
