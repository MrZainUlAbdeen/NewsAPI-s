using AutoMapper;
using MediatR;
using NewsBook.Mediator.Commands.FavouriteNewsCommands;
using NewsBook.Mediator.Response;
using NewsBook.Repository;
using NuGet.Protocol.Plugins;

namespace NewsBook.Mediator.Commands.FavouriteNewsCommand
{
    public class FavouriteNewsCommandsHandler : 
        IRequestHandler<MarkFavouriteNews, NewsReadDTO>,
        IRequestHandler<DeleteFavouriteNews, NewsReadDTO>
    {
        private readonly IFavouriteNewsRespository _favouriteNews;
        private IMapper _mapper;
        public FavouriteNewsCommandsHandler(IFavouriteNewsRespository favouriteNews, IMapper mapper)
        {
            _favouriteNews = favouriteNews;
            _mapper = mapper;
        }

        public async Task<NewsReadDTO> Handle(MarkFavouriteNews request, CancellationToken cancellationToken)
        {
            var favouriteNews = await _favouriteNews.Insert(
               request.id
           );
            return _mapper.Map<NewsReadDTO>(favouriteNews);
        }

        public async Task<NewsReadDTO> Handle(DeleteFavouriteNews request, CancellationToken cancellationToken)
        {
            var favouriteNews = await _favouriteNews.Delete(request.id);
            return _mapper.Map<NewsReadDTO>(favouriteNews);
        }
    }
}