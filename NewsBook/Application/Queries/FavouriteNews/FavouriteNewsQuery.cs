using MediatR;
using NewsBook.Models.Paging;
using NewsBook.Response;

namespace NewsBook.Mediator.Queries.FavouriteNews
{
    public class IBaseResponse<T> : IRequest<T>
    {
        public string OrderBy { get; set; } = string.Empty;
        public bool IsAscending { get; set; } = true;
    }
    public class GetFavouriteNewsQuery : IBaseResponse<List<NewsResponse>>
    {

    }
    public class GetPaginatedFavouriteNewsQuery : IBaseResponse<PagedList<NewsResponse>>
    {
        public PagingParameters Page;

    }
    public class GetFavouriteNewsByIdQuery : IBaseResponse<NewsResponse>
    {
        public Guid Id;
    }
}
