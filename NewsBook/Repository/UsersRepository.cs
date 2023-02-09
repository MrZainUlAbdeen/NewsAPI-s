using Microsoft.EntityFrameworkCore;
using NewsBook.Data;
using NewsBook.IdentityServices;
using NewsBook.Models;
using NewsBook.Models.Paging;

namespace NewsBook.Repository
{
    public class UsersRepository : NewsBase<User>, IUsersRepository
    {
        private readonly DatabaseContext dbContext;
        private readonly IIdentityServices _identityServices;
        public UsersRepository(
            DatabaseContext dbContext,
            IIdentityServices identityServices
            ) : base(dbContext)
        {
            _identityServices= identityServices;
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _identityServices = identityServices;
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
                Password = password
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

        public async Task<List<User>> GetAll()
        {
            return await FindAll().ToListAsync();
        }
        public async Task<PagedList<User>> GetAll(PagingParameters pagingParameters)
        {
            return await PagedList<User>.ToPagedList(
                FindAll(),
                pagingParameters.PageNumber,
                pagingParameters.PageSize
            );
        }

        public async Task<User> GetById(Guid id)
        {
            var user = await dbContext.Users.FindAsync(id);
            return user;
        }

        public async Task<User> GetByFilters(string email, string password)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(authenticate => 
            authenticate.Email == email &&
            authenticate.Password == password);
        }

        public async Task<User> Update(string name, string password)
        {
            var userId = _identityServices.GetUserId() ?? Guid.Empty;
            var user = await GetById(userId);
            if (user != null)
            {
                user.Name = name;
                user.Password = password;
                await Update(user);
                return user;
            }
            return null;
        }
    }
}