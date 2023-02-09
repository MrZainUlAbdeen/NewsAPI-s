using MediatR;
using NewsBook.Mediator.Response;
using NewsBook.Models.Paging;

namespace NewsBook.Mediator.Queries.FavouriteNews
{
    public class GetFavouriteNewsQuery : IRequest<List<NewsReadDTO>>
    {

    }
    public class GetPaginatedFavouriteNewsQuery : IRequest<PagedList<NewsReadDTO>>
    {
        public PagingParameters Page;
    }
    public class GetFavouriteNewsByIdQuery : IRequest<NewsReadDTO>
    {
        public Guid id;
    }
}
