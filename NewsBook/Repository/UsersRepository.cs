using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsBook.Application.Extensions;
using NewsBook.Core;
using NewsBook.Data;
using NewsBook.IdentityServices;
using NewsBook.Models;
using NewsBook.Models.Paging;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace NewsBook.Repository
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        //const int keySize = 64;
        //const int iterations = 350000;
        //HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        private readonly DatabaseContext dbContext;
        private readonly IIdentityServices _identityServices;
        private readonly INewsRepository _newsRepository;
        public UsersRepository(
            DatabaseContext dbContext,
            IIdentityServices identityServices,
            INewsRepository newsRepository
            ) : base(dbContext)
        {
            _newsRepository = newsRepository;
            _identityServices= identityServices;
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _identityServices = identityServices;
        }

        public string HashPasword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        public async Task<User?> CheckEmail(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(value => value.Email.Equals(email));
        }
        public async Task<User> Insert(string name, string email, string password)
        {
            var user = new User()
            {
                Name = name,
                Email = email,
                Password = HashPasword(password),
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> Update(User user)
        {
            _dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            return user;
        }
        public async Task<User> Delete(Guid id)
        {
            var user = await GetById(id);
            if (user != null) {
                dbContext.Users.Remove(user);
            }
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAll(string orderBy, bool isAscending = true)
        {
            var queryable = FindAll();
            if (!string.IsNullOrEmpty(orderBy))
            {
                OrderByPropertyOrField(queryable ,orderBy, isAscending);
            }
            return await queryable.ToListAsync();
        }

        public async Task<PagedList<User>> GetAll(string tableName ,
            string filterName, 
            string startDate, 
            string endDate,
            PagingParameters pagingParameters,
            string orderBy, 
            bool isAscending = true)
        {
            //Expression<Func<User, bool>> expressions;
            //expressions = user => user.Email == filterName;
            //expressions = user => user.Name == startDate;

            var queryable = FindAll();
            //queryable = GetWithFilter(expressions);
            //if (!string.IsNullOrEmpty(orderBy))
            //{
            //    queryable.OrderByPropertyOrField(orderBy, isAscending);
            //}
            //queryable = GetWithFilter(expressions);
            //queryable = GetOrderInBetween(startDate, endDate);
            //OrderByPropertyOrField(queryable, orderBy, isAscending);
            //queryable = (IQueryable<User>)GetFiltering(queryable, filterName, startDate );
            //queryable = Search(queryable, filterName);
            //queryable = Filter(expressions);

            queryable = GetFiltering(tableName, filterName, queryable);
            return await PagedList<User>.ToPagedList(
                queryable,
                pagingParameters.PageNumber,
                pagingParameters.PageSize
            );
        }

        public async Task<User> GetById(Guid id)
        {
            var user = await dbContext.Users.FindAsync(id);
            return user;
        }

        public Task<User> GetByFilters(string email, string password)
        {
            var user = _dbContext.Users.SingleOrDefaultAsync(authenticate =>
            authenticate.Email == email);
            var hashPassword = !VerifyPassword(user.Result.Password, HashPasword(password));
            if (user.Equals(null) || hashPassword.Equals(null))
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
            var fNewsQueryable = _dbContext.FavouriteNews.Select(fnews=> fnews);

            if (filterBy != null)
            {
                fNewsQueryable = fNewsQueryable.Where(filterBy);
            }
            var queryable = (from user in _dbContext.Users
                             join fnews in fNewsQueryable
                             on user.Id equals fnews.UserId
                             where fnews.IsFavorite == true && fnews.NewsId == NewsId
                             select user);


            return await PagedList<User>.ToPagedList(
                queryable,
                pagingParameters.PageNumber,
                pagingParameters.PageSize);
        }
    }
}