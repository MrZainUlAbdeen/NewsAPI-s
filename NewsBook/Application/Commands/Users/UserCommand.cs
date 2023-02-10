using MediatR;
using NewsBook.Response;
using System.ComponentModel.DataAnnotations;

namespace NewsBook.Mediator.Commands.Users
{
    public class InsertUserQuery : IRequest<UserResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UpdateUserQuery : IRequest<UserResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class DeleteUserQuery : IRequest<UserResponse>
    {
        public Guid Id;
    }
    public class UserLoginQuery : IRequest<string>
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
