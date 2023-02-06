using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsBook.Authorization;
using NewsBook.Data;
using NewsBook.IdentityServices;
using NewsBook.ModelDTO.User;
using NewsBook.Models;
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
        [Authorize(Role.admin)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _usersRepository.GetAll();
            return Ok(_mapper.Map<List<UserReadDTO>>(user));
        }
        [Authorize(Role.admin)]
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(Guid Id)
        {
            return Ok(await _usersRepository.GetById(Id));
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserWriteDTO UserDTO)
        {
            var user = await _usersRepository.Insert(UserDTO.Name, UserDTO.Email, UserDTO.Password);
            return Ok(_mapper.Map<UserReadDTO>(user));
        }
        [Authorize(Role.user)]
        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(Guid Id, [FromBody] UserWriteDTO UserDTO)
        {
            var user = await _usersRepository.GetById(Id);
            if (user == null) {
                return BadRequest(Id);
            }

            user.Name = UserDTO.Name;
            user.Email = UserDTO.Email;
            user.Password = UserDTO.Password;

            user = await _usersRepository.Update(user);
            return Ok(user);
        }
        [Authorize(Role.admin)]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var user = await _usersRepository.GetById(Id);
            if (user == null)
            {
                return BadRequest(Id);
            }

            return Ok(await _usersRepository.Delete(Id));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(UserAuthenticationDTO user)
        {
            var _user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == user.Email && x.Password == user.Password);
            if (_user == null)
            {
                return BadRequest("Invalid Email/Password");
            }
            var jwtToken = _jwtUtils.GenerateToken(_user);
            return Ok(jwtToken);
        }

        [HttpGet("loginUserId")]
        public async Task<IActionResult> UserId()
        {
            return Ok(_identityServices.GetUserId() ?? Guid.Empty);
        }
    }
}

