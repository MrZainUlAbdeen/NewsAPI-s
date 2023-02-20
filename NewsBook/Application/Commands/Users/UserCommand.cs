using MediatR;
using NewsBook.Response;
using System.ComponentModel.DataAnnotations;

namespace NewsBook.Mediator.Commands.Users
{
    public class InsertUserCommand : IRequest<UserResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UpdateUserCommand : IRequest<UserResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class DeleteUserCommand : IRequest<UserResponse>
    {
        public Guid Id;
    }
    public class UserLoginCommand : IRequest<string>
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
