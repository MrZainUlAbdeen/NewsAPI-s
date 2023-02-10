using AutoMapper;
using MediatR;
using NewsBook.IdentityServices;
using NewsBook.Models;
using NewsBook.Models.Paging;
using NewsBook.Repository;
using NewsBook.Response;
using NuGet.Protocol.Plugins;

namespace NewsBook.Mediator.Queries.Users
{
    public class UserQueryHandler :
        IRequestHandler<GetUsersQuery, List<User>>,
        IRequestHandler<GetPaginatedUsersQuery, PagedList<UserResponse>>,
        IRequestHandler<GetUserByIdQuery, UserResponse>

    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        
        public UserQueryHandler(
            IUsersRepository usersRepository,
            IMapper mapper
            
        )
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _usersRepository.GetAll(request.OrderBy, request.IsAscending);
        }

        public async Task<PagedList<UserResponse>> Handle(GetPaginatedUsersQuery request, CancellationToken cancellationToken)
        {
            var pagedUser = await _usersRepository.GetAll(request.Page, request.OrderBy, request.IsAscending);
            PagedList<UserResponse> pagedUsersDTO = new PagedList<UserResponse>
            {
                Items = _mapper.Map<List<UserResponse>>(pagedUser.Items),
                TotalCount = pagedUser.TotalCount,
                TotalPages = pagedUser.TotalPages,
                CurrentPage = pagedUser.CurrentPage,
                PageSize = pagedUser.PageSize
            };

            return pagedUsersDTO;
        }

        public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.GetById(request.Id);
            return _mapper.Map<UserResponse>(user);
        }
    }
}