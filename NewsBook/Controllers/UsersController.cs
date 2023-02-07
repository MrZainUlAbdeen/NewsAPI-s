using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsBook.Authorization;
using NewsBook.Data;
using NewsBook.IdentityServices;
using NewsBook.ModelDTO.News;
using NewsBook.ModelDTO.User;
using NewsBook.Models;
using NewsBook.Models.Paging;
using NewsBook.Repository;
using AllowAnonymousAttribute = NewsBook.Authorization.AllowAnonymousAttribute;
using AuthorizeAttribute = NewsBook.Authorization.AuthorizeAttribute;


namespace NewsBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IUsersRepository _usersRepository;
        private readonly IJwtUtils _jwtUtils;
        private readonly IIdentityServices _identityServices;
        private IMapper _mapper;

        public UsersController(
            IUsersRepository usersRepository,
            DatabaseContext dbContext, 
            IJwtUtils jwtUtils, 
            IIdentityServices identityServices,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _identityServices= identityServices;
            _jwtUtils= jwtUtils;
            _dbContext = dbContext;
            _usersRepository = usersRepository;
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
                var pagedUser = await _usersRepository.GetAll(pagingParameters);
                var pagedUsersDTO = new PagedList<UserReadDTO>
                {
                    Items = _mapper.Map<List<UserReadDTO>>(pagedUser.Items),
                    TotalCount = pagedUser.TotalCount,
                    TotalPages = pagedUser.TotalPages,
                    CurrentPage = pagedUser.CurrentPage,
                    PageSize = pagedUser.PageSize
                };
                return Ok(pagedUsersDTO);
            }
            var user = await _usersRepository.GetAll();
            return Ok(_mapper.Map<List<UserReadDTO>>(user));
        }

        [Authorize(Role.admin)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _usersRepository.GetById(id));
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserWriteDTO userDTO)
        {
            var email = await _dbContext.Users.FirstOrDefaultAsync(value => value.Email.Equals(userDTO.Email));
            if(email == null)
            {
                var user = await _usersRepository.Insert(userDTO.Name, userDTO.Email, userDTO.Password);
                return Ok(_mapper.Map<UserReadDTO>(user));
            }
            return BadRequest("Email already Exist");
        }
        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> Put(
            [FromBody] UserWriteDTO userDTO
            )
        {
            var userId = _identityServices.GetUserId() ?? Guid.Empty;
            if (userId.Equals(_usersRepository.GetById(userId))) 
            {
                await _usersRepository.Insert(userDTO.Name, userDTO.Email, userDTO.Password);
                return Ok(userId);
            }
            return BadRequest("User not found");
        }
        [Authorize(Role.admin)]
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete()
        {
            var userId = _identityServices.GetUserId() ?? Guid.Empty;
            return Ok(await _usersRepository.Delete(userId));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(UserAuthenticationDTO userAuthentication)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(
                x => x.Email == userAuthentication.Email && x.Password == userAuthentication.Password
                );
            if (user == null)
            {
                return BadRequest("Invalid Email/Password");
            }
            var jwtToken = _jwtUtils.GenerateToken(user);
            return Ok(jwtToken);
        }

        [HttpGet("loginUserId")]
        public IActionResult UserId()
        {
            return Ok(_identityServices.GetUserId() ?? Guid.Empty);
        }
    }
}

