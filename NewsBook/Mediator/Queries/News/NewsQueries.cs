using MediatR;
using NewsBook.Mediator.Response;
using NewsBook.Models.Paging;

namespace NewsBook.Mediator.Queries.News
{
    public class GetNewsQuery : IRequest<List<NewsBook.Models.News>>
    {

    }
    public class GetPaginatedNewsQuery : IRequest<PagedList<NewsReadDTO>>
    {
        public PagingParameters Page;
    }

    public class GetNewsByIdQuery : IRequest<NewsReadDTO>
    {
        public Guid Id;
    } 
}
