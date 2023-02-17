using Microsoft.EntityFrameworkCore;
using NewsBook.Core;
using NewsBook.Data;
using NewsBook.IdentityServices;
using NewsBook.Models;
using NewsBook.Models.Paging;
using System.Linq.Expressions;

namespace NewsBook.Repository
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        private readonly DatabaseContext _dbContext;
        private readonly IIdentityServices _identityServices;
        public UsersRepository(
            DatabaseContext dbContext,
            IIdentityServices identityServices
            ) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _identityServices = identityServices;
        }

        public async Task<User?> CheckEmail(string email)
        {
            return await base._dbContext.Users.FirstOrDefaultAsync(value => value.Email.Equals(email));
        }
        public async Task<User> Insert(string name, string email, string password)
        {
            var user = new User()
            {
                Name = name,
                Email = email,
                Password = HashPasword(password),
            };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> Update(User user)
        {
            base._dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
        public async Task<User> Delete(Guid id)
        {
            var user = await GetById(id);
            if (user != null) {
                _dbContext.Users.Remove(user);
            }
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAll(string tableAttribute, string filterName, string orderBy, bool isAscending = true)
        {
            var queryable = GetBySortFilter(tableAttribute, filterName, orderBy, isAscending);
            return await queryable.ToListAsync();
        }

        public async Task<PagedList<User>> GetAll(
            PagingParameters pagingParameters,
            string tableAttribute, 
            string filterName,
            string orderBy, 
            bool isAscending = true)
        {
            var queryable = GetBySortFilter(tableAttribute, filterName, orderBy, isAscending);
            return await PagedList<User>.ToPagedList(
                queryable,
                pagingParameters.PageNumber,
                pagingParameters.PageSize
            );
        }
        

        public async Task<User> GetById(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            return user;
        }

        public async Task<User> GetByFilters(string email, string password)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(authenticate =>
            authenticate.Email == email);
            var hashPassword = !VerifyPassword(user.Password, HashPasword(password));
            if (user.Equals(null) || hashPassword.Equals(false))
            {
                return null;
            }
            return user;
            
        }

        public async Task<User> Update(string name, string password)
        {
            var userId = _identityServices.GetUserId() ?? Guid.Empty;
            var user = await GetById(userId);
            if (user != null)
            {
                user.Name = name;
                user.Password = HashPasword(password);
                await Update(user);
                return user;
            }
            return null;
        }

        public async Task<PagedList<User>> GetByNewsId(
            Guid NewsId, 
            Expression<Func<FavouriteNews, bool>>? filterBy,
            PagingParameters pagingParameters)
        {
            var queryable = GetFilterByNewsId(NewsId, filterBy);
            return await PagedList<User>.ToPagedList(
                queryable,
                pagingParameters.PageNumber,
                pagingParameters.PageSize);
        }

        public async Task<List<User>> GetByNewsId(
            Guid NewsId,
            Expression<Func<FavouriteNews, bool>>? filterBy)
        {
            var queryable = GetFilterByNewsId(NewsId, filterBy);
            return await queryable.ToListAsync();
        }

        private IQueryable<User> GetFilterByNewsId(
             Guid NewsId,
            Expression<Func<FavouriteNews, bool>>? filterBy
            )
        {
            var fNewsQueryable = base._dbContext.FavouriteNews.Select(fnews => fnews);
            if (filterBy != null)
            {
                fNewsQueryable = fNewsQueryable.Where(filterBy);
            }
            var queryable = (from user in base._dbContext.Users
                             join fnews in fNewsQueryable
                             on user.Id equals fnews.UserId
                             where fnews.IsFavorite == true && fnews.NewsId == NewsId
                             select user);
            return queryable;
        }
        private IQueryable<User> GetBySortFilter(string tableAttribute, string filterName, string orderBy, bool isAscending = true)
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

        private string HashPasword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}