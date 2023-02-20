using MediatR;

namespace NewsBook.Application.Commands.Images
{
    public class BaseRequest<T> : IRequest<T>
    {
        public IFormFile Picture { get; set; }
    }
    public class InsertProfileCommand : BaseRequest<string>
    {
    }
    public class UpdateProfileCommand : BaseRequest<string>
    {

    }
}
