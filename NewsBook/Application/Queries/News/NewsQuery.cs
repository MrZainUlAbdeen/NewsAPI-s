using MediatR;
using NewsBook.Mediator.Response;
using NewsBook.Models.Paging;

namespace NewsBook.Mediator.Queries.News
{
    public class GetNewsQuery : IRequest<List<NewsBook.Models.News>>
    {

    }
    public class GetPaginatedNewsQuery : IRequest<PagedList<NewsResponse>>
    {
        public PagingParameters Page;
    }

    public class GetNewsByIdQuery : IRequest<NewsResponse>
    {
        public Guid Id;
    } 
}
