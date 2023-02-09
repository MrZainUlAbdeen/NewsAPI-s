using AutoMapper;
using MediatR;
using NewsBook.Mediator.Response;
using NewsBook.Models.Paging;
using NewsBook.Repository;

namespace NewsBook.Mediator.Queries.FavouriteNews
{
    public class FavouriteNewsQueryHandler : 
        IRequestHandler<GetFavouriteNewsQuery, List<NewsResponse>>,
        IRequestHandler<GetPaginatedFavouriteNewsQuery, PagedList<NewsResponse>>,
        IRequestHandler<GetFavouriteNewsByIdQuery, NewsResponse>
    {
        private readonly INewsRepository _newsRepository;
        private IMapper _mapper;
        private readonly IFavouriteNewsRespository _favouriteNewsRespository;
        public FavouriteNewsQueryHandler(IMapper mapper, INewsRepository newsRepository, IFavouriteNewsRespository favouriteNewsRespository)
        {
            _favouriteNewsRespository = favouriteNewsRespository;
            _mapper = mapper;
            _newsRepository = newsRepository;
        }

        public async Task<List<NewsResponse>> Handle(GetFavouriteNewsQuery request, CancellationToken cancellationToken)
        {
            var favouriteNews = await _newsRepository.GetFavouriteNews();
            return _mapper.Map<List<NewsResponse>>(favouriteNews);
        }

        public async Task<PagedList<NewsResponse>> Handle(GetPaginatedFavouriteNewsQuery request, CancellationToken cancellationToken)
        {
            //NOTE: Solution to map object to another object
            //var news = (await _newsRepository.GetFavouriteNews())
            //    .Select(
            //        newsDTO => new NewsReadDTO
            //        {
            //            NewsId = newsDTO.Id,
            //            UserId = newsDTO.UserId,
            //            Title = newsDTO.Title,
            //            Description = newsDTO.Description,
            //            CreatedAt = newsDTO.CreatedAt,
            //            UpdatedAt = newsDTO.UpdatedAt
            //        }
            //    );
            //return Ok(news);

            var pagedFavouriteNews = await _newsRepository.GetFavouriteNews(request.Page);
            var pagedNewsDTO = new PagedList<NewsResponse>
            {
                Items = _mapper.Map<List<NewsResponse>>(pagedFavouriteNews.Items),
                TotalCount = pagedFavouriteNews.TotalCount,
                TotalPages = pagedFavouriteNews.TotalPages,
                CurrentPage = pagedFavouriteNews.CurrentPage,
                PageSize = pagedFavouriteNews.PageSize
            };
            return pagedNewsDTO;
        }

        public async Task<NewsResponse> Handle(GetFavouriteNewsByIdQuery request, CancellationToken cancellationToken)
        {
            var favouritenews =  await _favouriteNewsRespository.GetById(request.Id);
            return _mapper.Map<NewsResponse>(favouritenews);
        }
    }
}
