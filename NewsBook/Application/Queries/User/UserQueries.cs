using MediatR;
using NewsBook.Mediator.Response;
using NewsBook.Models;
using NewsBook.Models.Paging;

namespace NewsBook.Mediator.Queries.Users
{
    public class GetUsersQuery : IRequest<List<User>>
    {
    }

    public class GetPaginatedUsersQuery : IRequest<PagedList<UserReadDTO>>
    {
        public PagingParameters Page;
    }

    public class GetUserByIdQuery : IRequest<UserReadDTO>
    {
        public Guid Id;
    }
}