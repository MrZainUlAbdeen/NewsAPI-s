using MediatR;
using NewsBook.Models;
using NewsBook.Models.Paging;
using NewsBook.Response;

namespace NewsBook.Mediator.Queries.Users
{
    public class BaseRequest<T> : IRequest<T>
    {
        public string OrderBy { get; set; } = string.Empty;
        public bool IsAscending { get; set; } = true;
    }

    public class GetPaginatedUsersFavouriteNewsQuery : IRequest<PagedList<UserResponse>>
    {
        public PagingParameters Page;
        public Guid NewsId;
        public string? StartDate;
        public string? EndDate;
    }
    public class GetUsersFavouriteNewsQuery : IRequest<List<UserResponse>>
    {
        public Guid NewsId;
        public string? StartDate;
        public string? EndDate;
    }

    public class GetUsersQuery : BaseRequest<List<User>>
    {
    }

    public class GetPaginatedUsersQuery : BaseRequest<PagedList<UserResponse>>
    {
        public PagingParameters Page;
        public Guid userId;
        public string StartTime;
        public string EndDate;
        public string tableName;
        public string filterName;
    }

    public class GetUserByIdQuery : BaseRequest<UserResponse>
    {
        public Guid Id;
    }
}