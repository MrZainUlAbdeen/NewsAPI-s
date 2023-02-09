using MediatR;
using NewsBook.Mediator.Response;

namespace NewsBook.Mediator.Commands.News
{
    public class InsertNewsQuery : IRequest<NewsResponse>
    {
        public string Tittle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateNewsQuery : IRequest<NewsResponse>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class DeleteNewsQuery : IRequest<NewsResponse>
    {
        public Guid Id;
    }
}
