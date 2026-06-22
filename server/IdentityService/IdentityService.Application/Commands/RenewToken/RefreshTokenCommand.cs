using IdentityService.Application.DTOs;
using MediatR;

namespace IdentityService.Application.Commands.RenewToken
{
    public record RefreshTokenCommand(
        string RefreshToken
        ) : IRequest<AuthResponseDto>;

}
