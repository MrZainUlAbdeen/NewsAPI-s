using AutoMapper;
using MediatR;
using NewsBook.Mediator.Queries.Users;
using NewsBook.Mediator.Response;
using NewsBook.Models;
using NewsBook.Models.Paging;
using NewsBook.Repository;

namespace NewsBook.Mediator.Queries.News
{
    public class NewsQueryHandler : 
        IRequestHandler<GetNewsQuery, List<Models.News>>,
        IRequestHandler<GetPaginatedNewsQuery, PagedList<NewsReadDTO>>,
        IRequestHandler<GetNewsByIdQuery, NewsReadDTO>

    {
        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;
        public NewsQueryHandler(INewsRepository newsRepository, IMapper mapper)
        {
            _mapper= mapper;
            _newsRepository = newsRepository;
        }
        public async Task<List<Models.News>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetAll();
        }

        public async Task<PagedList<NewsReadDTO>> Handle(GetPaginatedNewsQuery request, CancellationToken cancellationToken)
        {
            var pagedNews = await _newsRepository.GetAll(request.Page);
            var pagedNewsDTO = new PagedList<NewsReadDTO>
            {
                Items = _mapper.Map<List<NewsReadDTO>>(pagedNews.Items),
                TotalCount = pagedNews.TotalCount,
                TotalPages = pagedNews.TotalPages,
                CurrentPage = pagedNews.CurrentPage,
                PageSize = pagedNews.PageSize
            };
            return _mapper.Map<PagedList<NewsReadDTO>>(pagedNewsDTO);
        }

        public async Task<NewsReadDTO> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
        {
            var getById = await _newsRepository.GetById(request.Id);
            return _mapper.Map<NewsReadDTO>(getById);
        }
    }
}
