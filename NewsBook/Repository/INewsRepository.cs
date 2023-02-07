using NewsBook.ModelDTO;
using NewsBook.Models;
using NewsBook.Models.Paging;

namespace NewsBook.Repository
{
    public interface INewsRepository : IBaseRepository<News>
    {
        Task<News> Insert(string Tittle, string Description);
        Task<News> Update(News news);
        Task<News> Delete(Guid Id);
        Task<List<News>> GetAll();
        Task<PagedList<News>> GetAll(PagingParameters pagingParameters);
        Task<News?> GetById(Guid Id);
        Task<List<News>> GetFavouriteNews();
        Task<PagedList<News>> GetFavouriteNews(PagingParameters PagingParameters);
    }
}
