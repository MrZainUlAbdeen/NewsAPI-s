using AutoMapper;
using MediatR;
using NewsBook.Repository;
using NewsBook.Response;

namespace NewsBook.Mediator.Commands.News
{
    public class NewsCommandHandler :
        IRequestHandler<DeleteNewsQuery, NewsResponse>,
        IRequestHandler<InsertNewsQuery, NewsResponse>,
        IRequestHandler<UpdateNewsQuery, NewsResponse>
    {
        private readonly IMapper _mapper;
        private readonly INewsRepository _newsRepository;
        public NewsCommandHandler(IMapper mapper, INewsRepository newsRepository)
        {
            _mapper = mapper;
            _newsRepository = newsRepository;
        }
        public async Task<NewsResponse> Handle(DeleteNewsQuery request, CancellationToken cancellationToken)
        {
            var news = await _newsRepository.Delete(request.Id);
            return _mapper.Map<NewsResponse>(news);
        }

        public async Task<NewsResponse> Handle(InsertNewsQuery request, CancellationToken cancellationToken)
        {
            var news = await _newsRepository.Insert(request.Tittle, request.Description);
            return _mapper.Map<NewsResponse>(news);
        }

        public async Task<NewsResponse> Handle(UpdateNewsQuery request, CancellationToken cancellationToken)
        {
            var news = await _newsRepository.Update(request.Id,
                request.Title, request.Description);
            return _mapper.Map<NewsResponse>(news);
        }
    }
}