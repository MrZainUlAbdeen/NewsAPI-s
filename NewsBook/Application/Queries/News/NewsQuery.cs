using MediatR;
using NewsBook.Models.Paging;
using NewsBook.Response;

namespace NewsBook.Mediator.Queries.News
{

    public class BaseRequest<T> : IRequest<T>
    {
        public string? OrderBy;
        public bool IsAscending { get; set; } = true;
        public string? TableAttribute;
        public string? FilterName;
        public Guid? UserId;
        public DateTime? Startdate;
        public DateTime? Enddate;
    }

    public class GetNewsQuery : BaseRequest<List<Models.News>>
    {
    }
    public class GetPaginatedNewsQuery : BaseRequest<PagedList<NewsResponse>>
    {
        public PagingParameters Page;   
    }
    public class GetUserNewsQuery : BaseRequest<NewsResponse>
    {
    }
    public class GetPaginatedUserNewsQuery : BaseRequest<PagedList<NewsResponse>>
    {
        public PagingParameters Page;
    }

    public class GetNewsByIdQuery : IRequest<NewsResponse>
    {
        public Guid Id;
    }
}
