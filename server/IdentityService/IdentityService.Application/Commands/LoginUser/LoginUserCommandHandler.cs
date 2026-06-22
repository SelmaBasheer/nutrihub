using IdentityService.Application.DTOs;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using MediatR;
using Serilog;

namespace IdentityService.Application.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger = Log.ForContext<LoginUserCommandHandler>();

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> Handle(LoginUserCommand request, CancellationToken ct)
        {
            _logger.Information("Login attempt for email {Email}", request.Email);

            // Check user exists
            var user = await _userRepository.GetByEmailAsync(request.Email, ct);
            if (user is null)
                {
                _logger.Warning("Login failed — user not found {Email}", request.Email);
                throw new InvalidOperationException("Invalid email or password");
            }

            // Verify password
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
                {
                _logger.Warning("Login failed — invalid password {Email}", request.Email);
                throw new InvalidOperationException("Invalid email or password"); 
            }

            // Generate access token
            var accessToken = _tokenService.GenerateAccessToken(user);
            var expiresAt = DateTime.UtcNow.AddMinutes(60);

            // Generate refresh token
            var refreshTokenValue = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiry = _tokenService.GetRefreshTokenExpiry();
            var refreshToken = RefreshToken.Create(
                                    user.Id,
                                    refreshTokenValue,
                                    refreshTokenExpiry);

            // Save refresh token to DB
            await _refreshTokenRepository.AddAsync(refreshToken, ct);
            await _refreshTokenRepository.SaveChangesAsync(ct);

            // Build user DTO
            var userDto = new UserDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.PhoneNumber,
                user.Role,
                user.CreatedAt);

            _logger.Information("Login successful for {Email} with role {Role}",
            request.Email, user.Role);

            return new AuthResponseDto(
                accessToken,
                refreshTokenValue,
                expiresAt,
                userDto);
        }
    }
}
