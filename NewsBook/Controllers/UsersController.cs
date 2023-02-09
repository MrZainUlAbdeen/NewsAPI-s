using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsBook.IdentityServices;
using NewsBook.Mediator.Commands.Users;
using NewsBook.Mediator.Queries.Users;
using NewsBook.Mediator.Response;
using NewsBook.ModelDTO.User;
using NewsBook.Models;
using NewsBook.Models.Paging;
using AllowAnonymousAttribute = NewsBook.Authorization.AllowAnonymousAttribute;
using AuthorizeAttribute = NewsBook.Authorization.AuthorizeAttribute;


namespace NewsBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
  
        private readonly IIdentityServices _identityServices;
        private IMapper _mapper;
        private readonly ISender _mediator;

        public UsersController( 
            IIdentityServices identityServices,
            IMapper mapper,
            ISender mediator
            )
        {
            _mediator= mediator;
            _mapper = mapper;
            _identityServices= identityServices;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] bool usePaging, 
            [FromQuery] PagingParameters pagingParameters
        )
        {
            if(usePaging == true)
            {
                var paginatedUsers = await _mediator.Send(new GetPaginatedUsersQuery { 
                    Page = pagingParameters
                });
                return Ok(paginatedUsers);
            }

            var user = await _mediator.Send(new GetUsersQuery());
            return Ok(_mapper.Map<List<UserReadDTO>>(user));
        }

        [Authorize(Role.admin)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _mediator.Send(new GetUserByIdQuery { Id = id }));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserWriteDTO userDTO)
        {
            var user = await _mediator.Send(new PostUserQuery
            {
                userDTO = userDTO
            });
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> Put(
            [FromBody] UserWriteDTO userDTO
            )
        {
            var user = await _mediator.Send(new PostUserQuery
            {
                userDTO = userDTO
            });
            return Ok(user);    
        }
        //[Authorize(Role.admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteUserQuery { Id = id }));
            
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(UserAuthenticationDTO userAuthentication)
        {
            var user = await _mediator.Send(new LoginQuery { UserAuthenticate = userAuthentication});
            if(user == null)
            {
                return BadRequest("Incorrect Email/Password");
            }
            return Ok(user);

        }

        [HttpGet("loginUserId")]
        public IActionResult UserId()
        {
            return Ok(_identityServices.GetUserId() ?? Guid.Empty);
        }
    }
}