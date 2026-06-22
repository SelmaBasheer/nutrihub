using IdentityService.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Application.Commands.LoginUser
{
    public record LoginUserCommand(
        string Email,
        string Password) : IRequest<AuthResponseDto>;
}
