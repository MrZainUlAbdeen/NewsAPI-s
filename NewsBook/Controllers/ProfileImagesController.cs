using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsBook.Application.Commands.Images;
using NewsBook.Authorization;
using NewsBook.Mediator.Commands.Users;

namespace NewsBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileImagesController : ControllerBase
    {
        private ISender _mediator;
        public ProfileImagesController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("me")]
        public async Task<IActionResult> Post([FromForm] InsertProfileCommand insertProfile)
        {
            return Ok(new JsonResult(insertProfile));
            //return Ok(await _mediator.Send(insertProfile));
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromForm ] UpdateProfileCommand updateProfile)
        {
            return Ok(await _mediator.Send(updateProfile));
        }
    }
}
