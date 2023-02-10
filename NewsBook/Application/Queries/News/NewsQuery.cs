using MediatR;
using NewsBook.Models;
using NewsBook.Models.Paging;
using NewsBook.Response;

namespace NewsBook.Mediator.Queries.News
{

    public class BaseRequest<T> : IRequest<T>
    {
        public string OrderBy { get; set; } = string.Empty;
        public bool IsAscending { get; set; } = true;
    }

    public class GetNewsQuery : BaseRequest<List<NewsBook.Models.News>>
    {

    }
    public class GetPaginatedNewsQuery : BaseRequest<PagedList<NewsResponse>>
    {
        public PagingParameters Page;
    }

    public class GetNewsByIdQuery : IRequest<NewsResponse>
    {
        public Guid Id;
    }
}
