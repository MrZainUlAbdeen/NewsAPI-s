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
        Task<List<News>> GetAll(string tableAttribute, string filterName, string orderBy, bool isAscending);
        Task<PagedList<News>> GetAll(string tableAttribute, string filterName, PagingParameters pagingParameters, string orderBy, bool isAscending);
        Task<List<News>> GetByUsersNewsId(Guid? userId, Expression<Func<News, bool>>? filterBy);
        Task<PagedList<News>> GetByUsersNewsId(PagingParameters pagingParameters, Guid? userId, Expression<Func<News, bool>>? filterBy);
        Task<News?> GetById(Guid id);
        Task<List<News>> GetFavouriteNews(string orderBy, bool isAscending);
        Task<PagedList<News>> GetFavouriteNews(PagingParameters pagingParameters, string orderBy, bool isAscending);
        Task<News> Update(string title, string description);
    }
}
