using MediatR;
using NewsBook.Mediator.Response;
using NewsBook.ModelDTO.User;

namespace NewsBook.Mediator.Commands.Users
{
    public class PostUserQuery : IRequest<UserReadDTO>
    {
        public UserWriteDTO userDTO;
    }

    public class PutUserQuery : IRequest<UserReadDTO>
    {
        public UserWriteDTO userDTO;
    }

    public class DeleteUserQuery : IRequest<UserReadDTO>
    {
        public Guid Id;
    }
    public class LoginQuery : IRequest<string>
    {
        public UserAuthenticationDTO UserAuthenticate;
    }
}
