using IdentityService.Application.Commands.LoginUser;
using IdentityService.Application.Commands.RegisterUser;
using IdentityService.Application.Commands.RenewToken;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command,
            CancellationToken ct)
        {
            var userId = await _mediator.Send(command, ct);
            return CreatedAtAction(nameof(Register), new { id = userId }, new { userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command,
            CancellationToken ct)
        {
            var response = await _mediator.Send(command, ct);
            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(
            [FromBody] RefreshTokenCommand command,
            CancellationToken ct)
        {
            var response = await _mediator.Send(command, ct);
            return Ok(response);
        }
    }
}
