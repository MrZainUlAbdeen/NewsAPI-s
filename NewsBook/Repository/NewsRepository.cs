using Microsoft.EntityFrameworkCore;
using NewsBook.Data;
using NewsBook.Models;
using NewsBook.Models.Paging;

namespace NewsBook.Repository
{
    public class NewsRepository : NewsBase<News>, INewsRepository
    {
        private readonly DatabaseContext dbContext;
        public NewsRepository(DatabaseContext newsContext) : base(newsContext)
        {
            dbContext = newsContext;
        }
        public async Task<News> Insert(string Tittle, string Description)
        {
            var news = new News()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Title = Tittle,
                Description = Description
            };
            await dbContext.News.AddAsync(news);
            await dbContext.SaveChangesAsync();
            return news;
        }

        public async Task<News> Update(News news)
        {
            news.UpdatedAt = DateTime.Now;
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
            return Task.FromResult(PagedList<News>.ToPagedList(FindAll().OrderBy(N => N.Id), newsParameters.PageNumber, newsParameters.PageSize));
        }

        public async Task<List<News>> GetFavouriteNews(Guid UserId)
        {
            var favouriteNews = (
                from news in dbContext.News
                join fNews in dbContext.FavouriteNews
                on news.Id equals fNews.NewsId
                where fNews.IsFavorite == true && fNews.UserId == UserId
                select new News() { 
                    Id = news.Id,
                    Title = news.Title,
                    Description = news.Description,
                    CreatedAt = news.CreatedAt,
                    UpdatedAt = news.UpdatedAt
                }
            );

            return await favouriteNews.ToListAsync();
        }

        //public async Task<News> Create(PostNews postnews)
        //{
        //    var news = new News()
        //    {
        //        Id = Guid.NewGuid(),
        //        Tittle = postnews.Tittle,
        //        Describtion = postnews.Describtion,
        //        CreatedAt = DateTime.Now,
        //        UpdatedAt = DateTime.Now,

        //    };
        //    await newsContext.News.AddAsync(news);
        //    await newsContext.SaveChangesAsync();
        //    return news;
        //}
    }
}
