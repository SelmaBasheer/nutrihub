using IdentityService.Application.DTOs;
using MediatR;

namespace IdentityService.Application.Commands.LoginUser
{
    public record LoginUserCommand(
        string Email,
        string Password) : IRequest<AuthResponseDto>;
}
