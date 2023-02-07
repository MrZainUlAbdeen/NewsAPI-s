using Microsoft.EntityFrameworkCore;
using NewsBook.Data;
using NewsBook.IdentityServices;
using NewsBook.Models;
using NewsBook.Models.Paging;

namespace NewsBook.Repository
{
    public class NewsRepository : NewsBase<News>, INewsRepository
    {
        private readonly DatabaseContext _dbContext;
        private readonly IIdentityServices _identityServices;
        public NewsRepository(
            DatabaseContext newsContext, 
            IIdentityServices identityServices
            ) : base(newsContext)
        {
            _identityServices = identityServices;
            _dbContext = newsContext;
        }
        public async Task<News> Insert(string title, string description)
        {
            var news = new News()
            {
                Title = title,
                Description = description,
                UserId = _identityServices.GetUserId() ?? Guid.Empty,
            };
            await _dbContext.News.AddAsync(news);
            await _dbContext.SaveChangesAsync();
            return news;
        }

        public async Task<News> Update(News news)
        {
            await _dbContext.SaveChangesAsync();
            return news;
        }


        public async Task<News> Delete(Guid id)
        {
            var news = await GetById(id);
            if (news != null)
            {
                _dbContext.News.Remove(news);
            }

            await _dbContext.SaveChangesAsync();
            return news;
        }
        public async Task<News?> GetById(Guid id)
        {
            return await _dbContext.News.FindAsync(id);
        }

        public async Task<List<News>> GetAll()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<PagedList<News>> GetAll(PagingParameters pagingParameters)
        {
            return await PagedList<News>.ToPagedList(
                FindAll(), 
                pagingParameters.PageNumber, 
                pagingParameters.PageSize
            );
        }

        private IQueryable<News> GetFavouriteNewsQueryable(Guid userId)
        {
            return (
               from news in _dbContext.News
               join fNews in _dbContext.FavouriteNews
               on news.Id equals fNews.NewsId
               where fNews.IsFavorite == true && fNews.UserId == userId
               select new News()
               {
                   Id = news.Id,
                   Title = news.Title,
                   Description = news.Description,
                   UserId = userId,
                   CreatedAt = news.CreatedAt,
                   UpdatedAt = news.UpdatedAt
               }
            );
        }
        public async Task<List<News>> GetFavouriteNews()
        {
            var userId = _identityServices.GetUserId() ?? Guid.Empty;
            var favouriteNews = GetFavouriteNewsQueryable(userId);
            return await favouriteNews.ToListAsync();
        }
        public async Task<PagedList<News>> GetFavouriteNews(PagingParameters pagingParameters)
        {
            var userId = _identityServices.GetUserId() ?? Guid.Empty;
            return await PagedList<News>.ToPagedList(
                    GetFavouriteNewsQueryable(userId),
                    pagingParameters.PageNumber, 
                    pagingParameters.PageSize
            );
        }
    }
}
