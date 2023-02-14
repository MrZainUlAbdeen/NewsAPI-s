using AutoMapper;
using MediatR;
using NewsBook.IdentityServices;
using NewsBook.Models;
using NewsBook.Models.Paging;
using NewsBook.Repository;
using NewsBook.Response;
using NuGet.Protocol.Plugins;
using System.Linq.Expressions;

namespace NewsBook.Mediator.Queries.Users
{
    public class UserQueryHandler :
        IRequestHandler<GetUsersQuery, List<User>>,
        IRequestHandler<GetPaginatedUsersQuery, PagedList<UserResponse>>,
        IRequestHandler<GetUserByIdQuery, UserResponse>,
        IRequestHandler<GetPaginatedUsersFavouriteNewsQuery, PagedList<UserResponse>>
        //IRequestHandler<GetUsersFavouriteNewsQuery , List<UserResponse>>

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
            //Expression<Func<User, bool>> filterBy = news => news.Email == request.filterName;
            var pagedUser = await _usersRepository.GetAll(request.tableName, request.filterName , request.StartTime,request.EndDate, request.Page,request.OrderBy ,request.IsAscending);
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

        public async Task<PagedList<UserResponse>> Handle(GetPaginatedUsersFavouriteNewsQuery request, CancellationToken cancellationToken)
        {
            DateTime startDate = DateTime.Parse(request.StartDate);
            DateTime lastDate = DateTime.Parse(request.EndDate);
            Expression<Func<Models.FavouriteNews, bool>>? filterBy = fNews => fNews.CreatedAt >= startDate;
            filterBy = fnews => fnews.UpdatedAt <= lastDate;
            var users = await _usersRepository.GetByNewsId(request.NewsId, filterBy, request.Page);
            PagedList<UserResponse> pagedUsers = new PagedList<UserResponse>
            {
                Items = _mapper.Map<List<UserResponse>>(users.Items),
                TotalCount = users.TotalCount,
                TotalPages = users.TotalPages,
                CurrentPage = users.CurrentPage,
                PageSize = users.PageSize
            };
            return _mapper.Map<PagedList<UserResponse>>(pagedUsers);
        }

        //public Task<List<UserResponse>> Handle(GetUsersFavouriteNewsQuery request, CancellationToken cancellationToken)
        //{

        //    DateTime startDate = DateTime.Parse(request.StartDate);
        //    DateTime lastDate = DateTime.Parse(request.EndDate);
        //    Expression<Func<Models.FavouriteNews, bool>>? filterBy = fNews => fNews.CreatedAt >= startDate;
        //    filterBy = fnews => fnews.UpdatedAt <= lastDate;
        //    var users = await _usersRepository.GetByNewsId(request.NewsId, filterBy, request.Page);
        //    return _mapper.Map<List<UserResponse>>(users);
        //}
    }
}