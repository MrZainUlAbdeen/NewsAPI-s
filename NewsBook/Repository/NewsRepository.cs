using Microsoft.EntityFrameworkCore;
using NewsBook.Data;
using NewsBook.IdentityServices;
using NewsBook.ModelDTO;
using NewsBook.ModelDTO.News;
using NewsBook.Models;
using NewsBook.Models.Paging;

namespace NewsBook.Repository
{
    public class NewsRepository : NewsBase<News>, INewsRepository
    {
        private readonly DatabaseContext dbContext;
        private readonly IIdentityServices _identityServices;
        public NewsRepository(DatabaseContext newsContext, IIdentityServices identityServices) : base(newsContext)
        {
            _identityServices = identityServices;
            dbContext = newsContext;
        }
        public async Task<News> Insert(string Tittle, string Description)
        {
            var news = new News()
            {
                Title = Tittle,
                Description = Description,
                UserId = _identityServices.GetUserId() ?? Guid.Empty,
            };
            await dbContext.News.AddAsync(news);
            await dbContext.SaveChangesAsync();
            return news;
        }

        public async Task<News> Update(News news)
        {
            await dbContext.SaveChangesAsync();
            return news;
        }


        public async Task<News> Delete(Guid Id)
        {
            var news = await GetById(Id);
            if (news != null)
            {
                dbContext.News.Remove(news);
            }

            await dbContext.SaveChangesAsync();
            return news;
        }



        public async Task<List<News>> GetAll()
        {
            return await dbContext.News.ToListAsync();
        }

        public async Task<News?> GetById(Guid Id)
        {
            return await dbContext.News.FindAsync(Id);
        }

        public Task<PagedList<News>> GetAll(NewsParameters newsParameters)
        {
            return Task.FromResult(PagedList<News>.ToPagedList(
                FindAll().OrderBy(N => N.CreatedAt), newsParameters.PageNumber, newsParameters.PageSize)
            );
        }

        public async Task<List<News>> GetFavouriteNews()
        {
            var UserId = _identityServices.GetUserId() ?? Guid.Empty;
            var favouriteNews = (
               from news in dbContext.News
               join fNews in dbContext.FavouriteNews
               on news.Id equals fNews.NewsId
               where fNews.IsFavorite == true && fNews.UserId == UserId
               select new News ()
               {
                   Id = news.Id,
                   Title = news.Title,
                   Description = news.Description,
                   UserId = UserId,
                   CreatedAt = news.CreatedAt,
                   UpdatedAt = news.UpdatedAt
               }
            );

            return await favouriteNews.ToListAsync();
        }
    }
}
