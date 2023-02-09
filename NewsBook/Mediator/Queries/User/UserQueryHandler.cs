using AutoMapper;
using MediatR;
using NewsBook.IdentityServices;
using NewsBook.Mediator.Response;
using NewsBook.Models;
using NewsBook.Models.Paging;
using NewsBook.Repository;
using NuGet.Protocol.Plugins;

namespace NewsBook.Mediator.Queries.Users
{
    public class UserQueryHandler :
        IRequestHandler<GetUsersQuery, List<User>>,
        IRequestHandler<GetPaginatedUsersQuery, PagedList<UserReadDTO>>,
        IRequestHandler<GetUserByIdQuery, UserReadDTO>

    {
        private readonly IUsersRepository _usersRepository;
        private IMapper _mapper;
        private IIdentityServices _identityService;

        public UserQueryHandler(
            IUsersRepository usersRepository,
            IMapper mapper,
            IIdentityServices identityService
        )
        {
            _identityService = identityService;
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _usersRepository.GetAll();
        }

        public async Task<PagedList<UserReadDTO>> Handle(GetPaginatedUsersQuery request, CancellationToken cancellationToken)
        {
            var pagedUser = await _usersRepository.GetAll(request.Page);
            PagedList<UserReadDTO> pagedUsersDTO = new PagedList<UserReadDTO>
            {
                Items = _mapper.Map<List<UserReadDTO>>(pagedUser.Items),
                TotalCount = pagedUser.TotalCount,
                TotalPages = pagedUser.TotalPages,
                CurrentPage = pagedUser.CurrentPage,
                PageSize = pagedUser.PageSize
            };

            return pagedUsersDTO;
        }

        public async Task<UserReadDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.GetById(request.Id);
            return _mapper.Map<UserReadDTO>(user);
        }
    }
}
