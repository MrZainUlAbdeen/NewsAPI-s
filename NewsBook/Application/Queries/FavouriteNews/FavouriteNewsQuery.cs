using MediatR;
using NewsBook.Mediator.Response;
using NewsBook.Models.Paging;

namespace NewsBook.Mediator.Queries.FavouriteNews
{
    public class GetFavouriteNewsQuery : IRequest<List<NewsResponse>>
    {

    }
    public class GetPaginatedFavouriteNewsQuery : IRequest<PagedList<NewsResponse>>
    {
        public PagingParameters Page;
    }
    public class GetFavouriteNewsByIdQuery : IRequest<NewsResponse>
    {
        public Guid Id;
    }
}
