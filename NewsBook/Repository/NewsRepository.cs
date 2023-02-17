using Microsoft.EntityFrameworkCore;
using NewsBook.Data;
using NewsBook.IdentityServices;
using NewsBook.Models;
using NewsBook.Models.Paging;
using NewsBook.Application.Extensions;
using NewsBook.Core;
using System.Linq.Expressions;

namespace NewsBook.Repository
{
    public class NewsRepository : BaseRepository<News>, INewsRepository
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

        public async Task<News> Update(string title, string description)
        {
            var userId = _identityServices.GetUserId() ?? Guid.Empty;
            var news = await GetById(userId);
            if (news == null)
            {
                return null;
            }

            news.Title = title;
            news.Description = description;
            Update(news);
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

        public async Task<List<News>> GetAll(string tableAttribute, string filterName, string orderBy, bool isAscending)
        {
            var queryable = GetBySortFilter(tableAttribute, filterName, orderBy, isAscending);
            return await queryable.ToListAsync();
        }

        public async Task<PagedList<News>> GetAll(string tableAttribute, string filterName, PagingParameters pagingParameters, string orderBy, bool isAscending)
        {
            var queryable = GetBySortFilter(tableAttribute, filterName, orderBy, isAscending);
            return await PagedList<News>.ToPagedList(
                queryable,
                pagingParameters.PageNumber, 
                pagingParameters.PageSize
            );
        }

        
        public async Task<List<News>> GetFavouriteNews(string orderBy, bool isAscending = true)
        {
            var userId = _identityServices.GetUserId() ?? Guid.Empty;
            var favouriteNews = GetFavouriteNewsQueryable(userId);
            if (!string.IsNullOrEmpty(orderBy))
            {
                favouriteNews.OrderByPropertyOrField(orderBy, isAscending);
            }
            return await favouriteNews.ToListAsync();
        }
        public async Task<PagedList<News>> GetFavouriteNews(PagingParameters pagingParameters, string orderBy, bool isAscending)
        {
            var userId = _identityServices.GetUserId() ?? Guid.Empty;
            var favouriteNews = GetFavouriteNewsQueryable(userId);
            if (!string.IsNullOrEmpty(orderBy))
            {
                favouriteNews = OrderByPropertyOrField(favouriteNews, orderBy, isAscending);
            }
            return await PagedList<News>.ToPagedList(
                    favouriteNews,
                    pagingParameters.PageNumber, 
                    pagingParameters.PageSize
            );
        }

        public async Task<News> Update(News news)
        {
            _dbContext.News.Update(news);
            await _dbContext.SaveChangesAsync();
            return news;
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
               });
        }

        private IQueryable<News> GetBySortFilter(string tableAttribute, string filterName, string orderBy, bool isAscending = true)
        {
            var queryable = FindAll();
            if (!string.IsNullOrEmpty(orderBy))
            {
                queryable = OrderByPropertyOrField(queryable, orderBy, isAscending);
            }
            if (!string.IsNullOrEmpty(filterName) && !string.IsNullOrEmpty(tableAttribute))
            {
                queryable = GetWithFilter(tableAttribute, filterName, queryable);
            }
            return queryable;
        }

        public async Task<List<News>> GetByUsersNewsId(Guid? userId, Expression<Func<News, bool>>? filterBy)
        {
            var queryable = GetByUserId(userId, filterBy);
            return await queryable.ToListAsync();
        }
        public async Task<PagedList<News>> GetByUsersNewsId(PagingParameters pagingParameters, Guid? userId, Expression<Func<News, bool>>? filterBy)
        {
            var queryable = GetByUserId(userId, filterBy);
            return await PagedList<News>.ToPagedList(
                queryable,
                pagingParameters.PageNumber,
                pagingParameters.PageSize
            );
        }

        private IQueryable<News> GetByUserId(
            Guid? UeerId, Expression<Func<News, bool>>? filterBy)
        {
            var usersQueryable = _dbContext.Users.Select(users => users);
            var queryable = FindAll();
            if (filterBy != null)
            {
                queryable = queryable.Where(filterBy);
            }
            queryable = (from news in queryable
                             join user in usersQueryable
                             on news.UserId equals user.Id
                             where news.UserId == UeerId
                             select news);
            return queryable;
        }
    }
}