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