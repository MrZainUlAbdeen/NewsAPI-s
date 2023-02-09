using AutoMapper;
using MediatR;
using NewsBook.Mediator.Commands.FavouriteNewsCommands;
using NewsBook.Mediator.Response;
using NewsBook.Repository;

namespace NewsBook.Mediator.Commands.FavouriteNewsCommand
{
    public class FavouriteNewsCommandHandler : 
        IRequestHandler<MarkFavouriteNews, NewsResponse>,
        IRequestHandler<DeleteFavouriteNews, NewsResponse>
    {
        private readonly IFavouriteNewsRespository _favouriteNews;
        private IMapper _mapper;
        public FavouriteNewsCommandHandler(IFavouriteNewsRespository favouriteNews, IMapper mapper)
        {
            _favouriteNews = favouriteNews;
            _mapper = mapper;
        }

        public async Task<NewsResponse> Handle(MarkFavouriteNews request, CancellationToken cancellationToken)
        {
            var favouriteNews = await _favouriteNews.Insert(
               request.Id
           );
            return _mapper.Map<NewsResponse>(favouriteNews);
        }

        public async Task<NewsResponse> Handle(DeleteFavouriteNews request, CancellationToken cancellationToken)
        {
            var favouriteNews = await _favouriteNews.Delete(request.Id);
            return _mapper.Map<NewsResponse>(favouriteNews);
        }
    }
}