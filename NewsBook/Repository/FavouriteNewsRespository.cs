using Microsoft.EntityFrameworkCore;
using NewsBook.Data;
using NewsBook.IdentityServices;
using NewsBook.Models;

namespace NewsBook.Repository
{
    public class FavouriteNewsRespository : IFavouriteNewsRespository
    {
        private readonly DatabaseContext dbContext;
        private readonly INewsRepository newsRepository;
        private readonly IUsersRepository usersRepository;
        private readonly IIdentityServices _identityServices;
        public FavouriteNewsRespository(
            DatabaseContext dbContext,
            INewsRepository newsRepository,
            IUsersRepository usersRepository,
            IIdentityServices identityServices
            )
        {
            _identityServices= identityServices;
            this.dbContext = dbContext;
            this.newsRepository = newsRepository;
            this.usersRepository = usersRepository;
        }

        public async Task<FavouriteNews?> Delete(Guid Id)
        {
            var favouriteNews = await GetById(Id);
            if (favouriteNews != null)
            {
                dbContext.FavouriteNews.Remove(favouriteNews);
            }

            await dbContext.SaveChangesAsync();
            return favouriteNews;
        }

        public async Task<FavouriteNews?> GetByFilters(Guid newsId, Guid userId)
        {

            return await dbContext.FavouriteNews.SingleOrDefaultAsync(
                favouriteNews => favouriteNews.NewsId == newsId && favouriteNews.UserId == userId
            );
        }

        public async Task<FavouriteNews> Insert(Guid newsId, bool isFavourite=true)
        {
            var news = await newsRepository.GetById(newsId);
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
            await dbContext.FavouriteNews.AddAsync(favouriteNews);
            await dbContext.SaveChangesAsync();

            return favouriteNews;
        }

        public async Task<FavouriteNews> Update(FavouriteNews favouriteNews)
        {
            await dbContext.SaveChangesAsync();
            return favouriteNews;
        }
        public async Task<List<FavouriteNews>> GetAll()
        {
            return await dbContext.FavouriteNews.ToListAsync();
        }
        public async Task<FavouriteNews?> GetById(Guid Id)
        {
            return await dbContext.FavouriteNews.FindAsync(Id);
        }
    }
}
