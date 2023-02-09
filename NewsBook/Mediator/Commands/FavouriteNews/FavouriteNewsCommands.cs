using MediatR;
using NewsBook.Mediator.Response;

namespace NewsBook.Mediator.Commands.FavouriteNewsCommands
{
    public class MarkFavouriteNews : IRequest<NewsReadDTO>
    {
        public Guid id;
    }
    public class PutFavouriteNews : IRequest<NewsReadDTO>
    {
        public Guid id;
    }
    public class DeleteFavouriteNews : IRequest<NewsReadDTO>
    {
        public Guid id;
    }
}
