using NewsBook.Models.Paging;
using NewsBook.Models;

namespace NewsBook.Repository
{
    public interface IFavouriteNewsRespository : IBaseRepository<FavouriteNews>
    {
        Task<FavouriteNews> Insert(Guid newsId);
        Task<FavouriteNews> Update(FavouriteNews favouriteNews);
        Task<FavouriteNews?> Delete(Guid Id);
        Task<PagedList<FavouriteNews>> GetAll(PagingParameters newsParameters);
        Task<FavouriteNews?> GetById(Guid Id);
        Task<FavouriteNews?> GetByFilters(Guid newsId, Guid userId);
    }
}
