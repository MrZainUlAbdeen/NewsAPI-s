using NewsBook.Models.Paging;
using NewsBook.Models;

namespace NewsBook.Repository
{
    public interface IFavouriteNewsRespository
    {
        Task<FavouriteNews> Insert(Guid newsId, bool isFavourite=true);
        Task<FavouriteNews> Update(FavouriteNews favouriteNews);
        Task<FavouriteNews?> Delete(Guid Id);
        Task<List<FavouriteNews>> GetAll();
        Task<FavouriteNews?> GetById(Guid Id);
        Task<FavouriteNews?> GetByFilters(Guid newsId, Guid userId);

        
    }
}
