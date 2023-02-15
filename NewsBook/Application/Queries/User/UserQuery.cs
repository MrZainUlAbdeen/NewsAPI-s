using MediatR;
using NewsBook.Models;
using NewsBook.Models.Paging;
using NewsBook.Response;

namespace NewsBook.Mediator.Queries.Users
{
    public class BaseRequest<T> : IRequest<T>
    {
        public string? OrderBy { get; set; } = string.Empty;
        public bool IsAscending = true;
        public Guid NewsId { get; set; }
        public DateTime? StartDate;
        public DateTime? EndDate;
        public string? TableAttribute;
        public string? filterName;
    }

    public class GetPaginatedUsersFavouriteNewsQuery : BaseRequest<PagedList<UserResponse>>
    {
        public PagingParameters Page;
    }
    public class GetUsersFavouriteNewsQuery : BaseRequest<List<UserResponse>>
    {
    }

    public class GetUsersQuery : BaseRequest<List<User>>
    {
    }

    public class GetPaginatedUsersQuery : BaseRequest<PagedList<UserResponse>>
    {
        public PagingParameters Page;
    }

    public class GetUserByIdQuery : BaseRequest<UserResponse>
    {
        public Guid Id;
    }
}