using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsBook.Authorization;
using NewsBook.IdentityServices;
using NewsBook.Mediator.Queries.Users;
using NewsBook.Mediator.Response;
using NewsBook.Models;
using NewsBook.Repository;

namespace NewsBook.Mediator.Commands.Users
{
    public class UserCommandsHandler : 
        IRequestHandler<DeleteUserQuery, UserReadDTO>,
        IRequestHandler<PostUserQuery, UserReadDTO>,
        IRequestHandler<PutUserQuery, UserReadDTO>,
        IRequestHandler<LoginQuery, string>
    {
        private readonly IUsersRepository _usersRepository;
        private IMapper _mapper;
        private readonly IIdentityServices _identityServices;
        private readonly IJwtUtils _jwtUtils;
        public UserCommandsHandler(
            IUsersRepository usersRepository,
            IMapper mapper,
            IIdentityServices identityServices,
            IJwtUtils jwtUtils
        )
        {
            _jwtUtils = jwtUtils;
            _identityServices = identityServices;
            _usersRepository= usersRepository;
            _mapper= mapper;
        }

        public async Task<UserReadDTO> Handle(DeleteUserQuery request, CancellationToken cancellationToken)
        {
            //var userId = _identityServices.GetUserId() ?? Guid.Empty;
            var userId = await _usersRepository.Delete(request.Id);
            return _mapper.Map<UserReadDTO>(userId);
        }

        public async Task<UserReadDTO> Handle(PostUserQuery request, CancellationToken cancellationToken)
        {
            //var userEmail = await _usersRepository.CheckEmail(request.userDTO.Email);
            var user = await _usersRepository.Insert(
                request.userDTO.Name,
                request.userDTO.Email,
                request.userDTO.Password
                );
            return _mapper.Map<UserReadDTO>(user);
        }

        public async Task<UserReadDTO> Handle(PutUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.Insert(request.userDTO.Name, request.userDTO.Email, request.userDTO.Password);
            return _mapper.Map<UserReadDTO>(user);   
        }

        public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.GetByFilters(request.UserAuthenticate.Email, request.UserAuthenticate.Password);
            if (user == null)
            {
                return null;
            }
            var token = _jwtUtils.GenerateToken(user);
            return token;
        }
    }
}
