using NewsBook.Models.Paging;
using NewsBook.Models;

namespace NewsBook.Repository
{
    public interface IUsersRepository : IBaseRepository<User>
    {
        Task<User> Insert(string name, string email, string password);
        Task<User> Update(User user);
        Task<User> Update(string name, string password);
        Task<User> Delete(Guid id);
        Task<User?> CheckEmail(string email);
        Task<PagedList<User>> GetAll(PagingParameters pagingParameters);
        Task<List<User>> GetAll();
        Task<User> GetById(Guid id);
        Task<User> GetByFilters(string email, string password);
    }
}
