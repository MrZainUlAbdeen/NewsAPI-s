using NewsBook.Models.Paging;
using NewsBook.Models;
using NewsBook.ModelDTO;

namespace NewsBook.Repository
{
    public interface IUsersRepository
    {
        Task<User> Insert(string Name, string Email, string Password);
        Task<User> Update(User user);
        Task<User> Delete(Guid Id);
        Task<List<User>> GetAll();
        Task<User> GetById(Guid Id);
        Task<User> GetByFilters(string Email, string Password);
    }
}
