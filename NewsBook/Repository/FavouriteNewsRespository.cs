using Microsoft.EntityFrameworkCore;
using NewsBook.Data;
using NewsBook.Models;

namespace NewsBook.Repository
{
    public class FavouriteNewsRespository : IFavouriteNewsRespository
    {
        private readonly DatabaseContext dbContext;
        private readonly INewsRepository newsRepository;
        private readonly IUsersRepository usersRepository;
        public FavouriteNewsRespository(DatabaseContext dbContext, INewsRepository newsRepository, IUsersRepository usersRepository)
        {
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

        public async Task<FavouriteNews> Insert(Guid newsId, Guid userId, bool isFavourite=true)
        {
            var news = await newsRepository.GetById(newsId);
            var user = await usersRepository.GetById(userId);
            if (user == null)
            {
                throw new Exception("UserId does not exists");
            }

            if (news == null)
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
                Id = Guid.NewGuid(),
                UserId = userId,
                NewsId = newsId,
                IsFavorite = isFavourite,
                //CreatedAt = new DateTime(),
                //UpdatedAt = new DateTime()
            };
            await dbContext.FavouriteNews.AddAsync(favouriteNews);
            await dbContext.SaveChangesAsync();

            return favouriteNews;
        }

        public async Task<FavouriteNews> Update(FavouriteNews favouriteNews)
        {
            //favouriteNews.UpdatedAt = DateTime.Now;
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
