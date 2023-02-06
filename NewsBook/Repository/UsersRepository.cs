using Microsoft.EntityFrameworkCore;
using NewsBook.Data;
using NewsBook.Models;
namespace NewsBook.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DatabaseContext dbContext;
       
        public UsersRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<User> Insert(string Name, string Email, string Password)
        {
            var user = new User()
            {
                Name = Name,
                Email = Email,
                Password = Password
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
            }

        public async Task<User> Update(User user)
        {
            await dbContext.SaveChangesAsync();
            return user;
        }
        public async Task<User> Delete(Guid Id)
        {
            var user = await GetById(Id);
            if (user != null) {
                dbContext.Users.Remove(user);
            }

            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAll()
        {
            
            return await dbContext.Users.ToListAsync();
        }

        public async Task<User> GetById(Guid Id)
        {
            var user = await dbContext.Users.FindAsync(Id);
            return user;
        }

        public  Task<User> GetByFilters(string Email, string Password)
        {
           throw new NotImplementedException();
        }
    }
}
