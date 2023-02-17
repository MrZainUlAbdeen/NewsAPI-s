using MediatR;
using NewsBook.Response;

namespace NewsBook.Mediator.Commands.FavouriteNewsCommands
{
    public class MarkFavouriteNews : IRequest<NewsResponse>
    {
        public Guid Id;
    }
    public class UpdateFavouriteNews : IRequest<NewsResponse>
    {
        public Guid Id;
    }
    public class DeleteFavouriteNews : IRequest<NewsResponse>
    {
        public Guid Id;
    }
}
