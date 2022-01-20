using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ms.Arch.Hw02.Api.Application.Infrastructure.Interfaces;
using Ms.Arch.Hw02.Api.ModelsDto;

namespace Ms.Arch.Hw02.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(
            IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            CancellationToken ct)
        {
            var userModels = await _userService.Get(ct);

            var userTvp = userModels.Select(user => new UserDto(user)).ToList();

            return Ok(userTvp);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] UserDto userDto,
            CancellationToken ct)
        {
            var userModel = userDto.ConvertToUserModel();
            var user = await _userService.Create(userModel, ct);

            return CreatedAtAction(nameof(Get), new {user.Id}, user);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> Get(
            [FromRoute] long userId,
            CancellationToken ct)
        {
            var userModel = await _userService.Get(userId, ct);

            if (userModel == null)
                return NotFound();

            var userDto = new UserDto(userModel);

            return Ok(userDto);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> Update(
            [FromRoute] long userId,
            [FromBody] UserDto userDto,
            CancellationToken ct)
        {
            if (!userDto.Id.Equals(userId))
                return BadRequest();

            var userModel = userDto.ConvertToUserModel();
            var user = await _userService.Update(userModel, ct);

            return Ok(user);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(
            [FromRoute] long userId,
            CancellationToken ct)
        {
            await _userService.Delete(userId, ct);

            return Ok();
        }
    }
}