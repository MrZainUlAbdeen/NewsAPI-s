using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsBook.Mediator.Commands.Users;
using NewsBook.Mediator.Queries.Users;
using NewsBook.Models;
using NewsBook.Models.Paging;
using NewsBook.Response;
using AllowAnonymousAttribute = NewsBook.Authorization.AllowAnonymousAttribute;
using AuthorizeAttribute = NewsBook.Authorization.AuthorizeAttribute;


namespace NewsBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
  
        private readonly IMapper _mapper;
        private readonly ISender _mediator;

        public UsersController(
            IMapper mapper,
            ISender mediator
            )
        {
            _mediator= mediator;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] bool usePaging,
            [FromQuery] bool isAscending,
            [FromQuery] string? orderBy,
            [FromQuery] Guid newsID,
            [FromQuery] string tableName,
            [FromQuery] string filterList,
            [FromQuery] string? startDate,
            [FromQuery] string? endDate,
            [FromQuery] PagingParameters pagingParameters
        )
        {
            if(usePaging == true)
            {
                var paginatedUsers = await _mediator.Send(new GetPaginatedUsersQuery {
                    Page = pagingParameters,
                    StartTime = startDate,
                    tableName=tableName,
                    filterName = filterList,
                    EndDate = endDate,
                    OrderBy = orderBy,
                    IsAscending = isAscending
                });
                return Ok(paginatedUsers);
            }

            var user = await _mediator.Send(new GetUsersQuery {
                OrderBy = orderBy,
                IsAscending = isAscending
            });
            return Ok(_mapper.Map<List<UserResponse>>(user));
        }

        [HttpGet("(favouritenews/id)")]
        public async Task<IActionResult> GetFavourite(
            [FromQuery] bool usePaging,
            [FromQuery] PagingParameters pagingParameters,
            Guid newsId, 
            string? startDate, 
            string? endDate)
        {
            if (usePaging == true)
            {
                var user = await _mediator.Send(new GetPaginatedUsersFavouriteNewsQuery
                {
                    Page = pagingParameters,
                    NewsId = newsId,
                    StartDate = startDate,
                    EndDate = endDate
                });
                return Ok(user);
            }
            return Ok("Please Select Usepagination");
        }

        [Authorize(Role.admin)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _mediator.Send(new GetUserByIdQuery { Id = id }));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InsertUserQuery insertUser)
        {            
            return Ok(await _mediator.Send(insertUser));
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> Put(
            [FromBody] UpdateUserQuery updateUser
            )
        {
            return Ok(await _mediator.Send(updateUser));
        }
        [Authorize(Role.admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteUserQuery { Id = id }));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(UserLoginQuery userLogin)
        {
            var user = await _mediator.Send(userLogin);
            if(user == null)
            {
                return BadRequest("Incorrect Email/Password");
            }
            return Ok(user);

        }
    }
}