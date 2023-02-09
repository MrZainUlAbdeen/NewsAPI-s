using NewsBook.Models.Paging;
using NewsBook.ModelDTO;
using NewsBook.Models;

namespace NewsBook.Repository
{
    public interface IUsersRepository : IBaseRepository<User>
    {
        Task<User> Insert(string Name, string Email, string Password);
        Task<User> Update(User user);
        Task<User> Delete(Guid Id);
        Task<User?> CheckEmail(string email);
        Task<PagedList<User>> GetAll(PagingParameters PagingParameters);
        Task<List<User>> GetAll();
        Task<User> GetById(Guid Id);
        Task<User> GetByFilters(string Email, string Password);
    }
}
