﻿using NewsBook.Models.Paging;
using NewsBook.Models;
using NewsBook.Core;
using System.Linq.Expressions;

namespace NewsBook.Repository
{
    public interface IUsersRepository : IBaseRepository<User>
    {
        Task<User> Insert(string name, string email, string password);
        Task<User> Update(User user);
        Task<User> Update(string name, string password);
        Task<User> Delete(Guid id);
        Task<User?> CheckEmail(string email);
        Task<PagedList<User>> GetAll(PagingParameters pagingParameters, string tableAttribute, string filterName,string orderBy, bool isAscending = true);
        Task<List<User>> GetAll(string tableAttribute, string filterName, string orderBy, bool isAscending = true);
        Task<PagedList<User>> GetByNewsId(
            Guid NewsId, 
            Expression<Func<FavouriteNews, bool>>? filterBy,
            PagingParameters pagingParameters);
        Task<List<User>> GetByNewsId(
            Guid NewsId,
            Expression<Func<FavouriteNews, bool>>? filterBy);
        Task<User> GetById(Guid id);
        Task<User> GetByFilters(string email, string password);
    }
}
