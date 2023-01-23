using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsBook.Data;
using NewsBook.ModelDTO;
using NewsBook.Repository;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewsBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersRepository _usersRepository;
        private IJwtUtils _jWTAuthentiation;
        public UsersController(
            IUsersRepository usersRepository,
            IJwtUtils jWTAuthentiation
        )
        {
            _usersRepository = usersRepository;
            _jWTAuthentiation = jWTAuthentiation;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _usersRepository.GetAll());
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(Guid Id)
        {
            return Ok(await _usersRepository.GetById(Id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserWriteDTO UserDTO)
        {
            var user = await _usersRepository.Insert(UserDTO.Name, UserDTO.Email, UserDTO.Password);
            return Ok(user);
        }

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
        public IActionResult Authenticate([FromBody] UserAuthenicaticationDTO user)
        {
            var token = _jWTAuthentiation.GenerateToken(user);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }

    }
}

