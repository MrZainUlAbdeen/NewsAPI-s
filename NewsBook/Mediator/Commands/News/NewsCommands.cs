using MediatR;
using NewsBook.Mediator.Response;
using NewsBook.ModelDTO.News;

namespace NewsBook.Mediator.Commands.News
{
    public class PostNewsQuery : IRequest<NewsReadDTO>
    {
        public NewsWriteDTO newsDTO;
    }

    public class PutNewsQuery : IRequest<NewsBook.Models.News>
    {
        public NewsUpdateRequest newsUpdate;
    }

    public class DeleteNewsQuery : IRequest<NewsReadDTO>
    {
        public Guid Id;
    }
}
