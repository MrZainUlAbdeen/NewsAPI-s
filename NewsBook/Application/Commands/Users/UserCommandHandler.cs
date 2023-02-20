using AutoMapper;
using MediatR;
using NewsBook.Authorization;
using NewsBook.Repository;
using NewsBook.Response;

namespace NewsBook.Mediator.Commands.Users
{
    public class UserCommandHandler : 
        IRequestHandler<DeleteUserCommand, UserResponse>,
        IRequestHandler<InsertUserCommand, UserResponse>,
        IRequestHandler<UpdateUserCommand, UserResponse>,
        IRequestHandler<UserLoginCommand, string>
    {
        private readonly IUsersRepository _usersRepository;
        private IMapper _mapper;
        private readonly IJwtUtils _jwtUtils;
        
        public UserCommandHandler(
            IUsersRepository usersRepository,
            IMapper mapper,
            IJwtUtils jwtUtils
        )
        {
            _jwtUtils = jwtUtils;
            _usersRepository= usersRepository;
            _mapper= mapper;
        }

        public async Task<UserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userId = await _usersRepository.Delete(request.Id);
            return _mapper.Map<UserResponse>(userId);
        }

        public async Task<UserResponse> Handle(InsertUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.Insert(
                request.Name,
                request.Email,
                request.Password
            );

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.Update(request.Name, request.Password);
            return _mapper.Map<UserResponse>(user);   
        }

        public async Task<string> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.GetByFilters(request.Email, request.Password);
            if (user == null)return null;
            return _jwtUtils.GenerateToken(user);
        }
    }
}
