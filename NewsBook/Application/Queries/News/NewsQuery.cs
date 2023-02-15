using MediatR;
using NewsBook.Models.Paging;
using NewsBook.Response;

namespace NewsBook.Mediator.Queries.News
{

    public class BaseRequest<T> : IRequest<T>
    {
        public string OrderBy { get; set; } = string.Empty;
        public bool IsAscending { get; set; } = true;
        //for Filtering
        public Guid UserId { get; set; } = Guid.Empty;
        public string Startdate { get; set; } = string.Empty;
        public string Lastdate { get; set; } = string.Empty;
    }

    public class GetNewsQuery : BaseRequest<List<Models.News>>
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
