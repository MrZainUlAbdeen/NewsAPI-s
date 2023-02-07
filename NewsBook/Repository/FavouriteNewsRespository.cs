using Microsoft.EntityFrameworkCore;
using NewsBook.Data;
using NewsBook.IdentityServices;
using NewsBook.Models;
using NewsBook.Models.Paging;

namespace NewsBook.Repository
{
    public class FavouriteNewsRespository : NewsBase<FavouriteNews>, IFavouriteNewsRespository
    {
        private readonly DatabaseContext _dbContext;
        private readonly INewsRepository _newsRepository;
        private readonly IIdentityServices _identityServices;
        public FavouriteNewsRespository(
            DatabaseContext dbContext,
            INewsRepository newsRepository,
            IIdentityServices identityServices
            ) : base( dbContext )
        {
            _identityServices= identityServices;
            _dbContext = dbContext;
            _newsRepository = newsRepository;
        }

        public async Task<FavouriteNews?> Delete(Guid id)
        {
            var favouriteNews = await GetById(id);
            if (favouriteNews != null)
            {
                _dbContext.FavouriteNews.Remove(favouriteNews);
            }

            await _dbContext.SaveChangesAsync();
            return favouriteNews;
        }

        public async Task<FavouriteNews?> GetByFilters(Guid newsId, Guid userId)
        {

            return await _dbContext.FavouriteNews.SingleOrDefaultAsync(
                favouriteNews => favouriteNews.NewsId == newsId && favouriteNews.UserId == userId
            );
        }

        public async Task<FavouriteNews> Insert(Guid newsId, bool isFavourite = true)
        {
            var news = await _newsRepository.GetById(newsId);
            var userId = _identityServices.GetUserId() ?? Guid.Empty;
            if (userId.Equals(null))
            {
                throw new Exception("UserId does not exists");
            }

            if (news.Equals(null))
            {
                throw new Exception("NewsId does not exists");
            }

            var favouriteNews = await GetByFilters(newsId, userId);

            if (favouriteNews != null && favouriteNews.IsFavorite != isFavourite)
            {
                favouriteNews.IsFavorite = isFavourite;
                await Update(favouriteNews);
            }

            favouriteNews = new FavouriteNews()
            {
                UserId = userId,
                NewsId = newsId,
                IsFavorite = isFavourite,
            };
            await _dbContext.FavouriteNews.AddAsync(favouriteNews);
            await _dbContext.SaveChangesAsync();

            return favouriteNews;
        }

        public async Task<FavouriteNews> Update(FavouriteNews favouriteNews)
        {
            await _dbContext.SaveChangesAsync();
            return favouriteNews;
        }
        public async Task<PagedList<FavouriteNews>> GetAll(PagingParameters pagingParameters)
        {
            return await PagedList<FavouriteNews>.ToPagedList(
                FindAll(),
                pagingParameters.PageNumber, 
                pagingParameters.PageSize
            );
        }
        public async Task<FavouriteNews?> GetById(Guid id)
        {
            return await _dbContext.FavouriteNews.FindAsync(id);
        }
    }
}
