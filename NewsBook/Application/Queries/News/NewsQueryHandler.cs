using AutoMapper;
using MediatR;
using NewsBook.Mediator.Queries.Users;
using NewsBook.Models;
using NewsBook.Models.Paging;
using NewsBook.Repository;
using NewsBook.Response;
using NuGet.Protocol.Plugins;
using System.Linq.Expressions;

namespace NewsBook.Mediator.Queries.News
{
    public class NewsQueryHandler : 
        IRequestHandler<GetNewsQuery, List<Models.News>>,
        IRequestHandler<GetPaginatedNewsQuery, PagedList<NewsResponse>>,
        IRequestHandler<GetNewsByIdQuery, NewsResponse>
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
            Expression<Func<Models.News, bool>> filterBy = news => news.UserId == request.UserId;
            return await _newsRepository.GetAll(request.OrderBy, request.IsAscending, filterBy);
        }

        public async Task<PagedList<NewsResponse>> Handle(GetPaginatedNewsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Models.News, bool>> filterBy = news => news.UserId == request.UserId;
            var pagedNews = await _newsRepository.GetAll(request.Page, request.OrderBy, request.IsAscending, filterBy);
            var pagedNewsDTO = new PagedList<NewsResponse>
            {
                Items = _mapper.Map<List<NewsResponse>>(pagedNews.Items),
                TotalCount = pagedNews.TotalCount,
                TotalPages = pagedNews.TotalPages,
                CurrentPage = pagedNews.CurrentPage,
                PageSize = pagedNews.PageSize
            };
            return _mapper.Map<PagedList<NewsResponse>>(pagedNewsDTO);
        }

        public async Task<NewsResponse> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
        {
            var getById = await _newsRepository.GetById(request.Id);
            return _mapper.Map<NewsResponse>(getById);
        }

    }
}
