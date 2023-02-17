using NewsBook.Models.Paging;
using NewsBook.Models;
using NewsBook.Core;

namespace NewsBook.Repository
{
    public interface IFavouriteNewsRespository : IBaseRepository<FavouriteNews>
    {
        Task<FavouriteNews> Insert(Guid newsId, bool isFavourite = true);
        Task<FavouriteNews> Update(FavouriteNews favouriteNews);
        Task<FavouriteNews?> Delete(Guid Id);
        Task<PagedList<FavouriteNews>> GetAll(PagingParameters newsParameters);
        Task<FavouriteNews?> GetById(Guid Id);
        Task<FavouriteNews?> GetByFilters(Guid newsId, Guid userId);
    }
}
