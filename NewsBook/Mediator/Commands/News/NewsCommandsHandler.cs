using AutoMapper;
using MediatR;
using NewsBook.Mediator.Queries.News;
using NewsBook.Mediator.Queries.Users;
using NewsBook.Mediator.Response;
using NewsBook.Repository;

namespace NewsBook.Mediator.Commands.News
{
    public class NewsCommandsHandler :
        IRequestHandler<DeleteNewsQuery, NewsReadDTO>,
        IRequestHandler<PostNewsQuery, NewsReadDTO>,
        IRequestHandler<PutNewsQuery, NewsBook.Models.News>
    {
        private readonly IMapper _mapper;
        private readonly INewsRepository _newsRepository;
        public NewsCommandsHandler(IMapper mapper, INewsRepository newsRepository)
        {
            _mapper = mapper;
            _newsRepository = newsRepository;
        }
        public async Task<NewsReadDTO> Handle(DeleteNewsQuery request, CancellationToken cancellationToken)
        {
            var news = await _newsRepository.Delete(request.Id);
            return _mapper.Map<NewsReadDTO>(news);
        }

        public async Task<NewsReadDTO> Handle(PostNewsQuery request, CancellationToken cancellationToken)
        {
            var news = await _newsRepository.Insert(request.newsDTO.Tittle, request.newsDTO.Description);
            return _mapper.Map<NewsReadDTO>(news);
        }

        public async Task<Models.News> Handle(PutNewsQuery request, CancellationToken cancellationToken)
        {
            var news = await _newsRepository.Update(
                request.newsUpdate.Id,
                request.newsUpdate.Title,
                request.newsUpdate.Description
                );
            return news;
        }
    }
}
