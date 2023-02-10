using NewsBook.Models.Paging;
using NewsBook.Models;
using NewsBook.Core;

namespace NewsBook.Repository
{
    public interface IUsersRepository : IBaseResponse<User>
    {
        Task<User> Insert(string name, string email, string password);
        Task<User> Update(User user);
        Task<User> Update(string name, string password);
        Task<User> Delete(Guid id);
        Task<User?> CheckEmail(string email);
        Task<PagedList<User>> GetAll(PagingParameters pagingParameters, string orderBy, bool isAscending = true);
        Task<List<User>> GetAll(string orderBy, bool isAscending = true);
        Task<User> GetById(Guid id);
        Task<User> GetByFilters(string email, string password);
    }
}
