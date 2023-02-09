using AutoMapper;
using MediatR;
using NewsBook.Mediator.Response;
using NewsBook.Models.Paging;
using NewsBook.Repository;

namespace NewsBook.Mediator.Queries.FavouriteNews
{
    public class FavouriteNewsQueryHandler : 
        IRequestHandler<GetFavouriteNewsQuery, List<NewsReadDTO>>,
        IRequestHandler<GetPaginatedFavouriteNewsQuery, PagedList<NewsReadDTO>>,
        IRequestHandler<GetFavouriteNewsByIdQuery, NewsReadDTO>
    {
        private readonly INewsRepository _newsRepository;
        private IMapper _mapper;
        public FavouriteNewsQueryHandler(IMapper mapper, INewsRepository newsRepository)
        {
            _mapper = mapper;
            _newsRepository = newsRepository;
        }

        public async Task<List<NewsReadDTO>> Handle(GetFavouriteNewsQuery request, CancellationToken cancellationToken)
        {
            var favouriteNews = await _newsRepository.GetFavouriteNews();
            return _mapper.Map<List<NewsReadDTO>>(favouriteNews);
        }

        public async Task<PagedList<NewsReadDTO>> Handle(GetPaginatedFavouriteNewsQuery request, CancellationToken cancellationToken)
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
            var pagedNewsDTO = new PagedList<NewsReadDTO>
            {
                Items = _mapper.Map<List<NewsReadDTO>>(pagedFavouriteNews.Items),
                TotalCount = pagedFavouriteNews.TotalCount,
                TotalPages = pagedFavouriteNews.TotalPages,
                CurrentPage = pagedFavouriteNews.CurrentPage,
                PageSize = pagedFavouriteNews.PageSize
            };
            return pagedNewsDTO;
        }

        public Task<NewsReadDTO> Handle(GetFavouriteNewsByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
