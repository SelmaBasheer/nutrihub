using IdentityService.Application.DTOs;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Repositories;
using MediatR;
using Serilog;


namespace IdentityService.Application.Commands.RenewToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger = Log.ForContext<RefreshTokenCommandHandler>();

        public RefreshTokenCommandHandler(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            _logger.Information("Token refresh attempt");

            // Find refresh token in DB
            var existingToken = await _refreshTokenRepository
                .GetByTokenAsync(request.RefreshToken, ct);

            if (existingToken is null)
            {
                _logger.Warning("Token refresh failed — invalid token");
                throw new InvalidOperationException("Invalid refresh token");
            }

            // Check if expired
            if (existingToken.ExpiresAt < DateTime.UtcNow)
            {
                _logger.Warning("Token refresh failed — token expired for user {UserId}",
                existingToken.UserId);
                throw new InvalidOperationException("Refresh token has expired");
            }

            // Get user
            var user = await _userRepository.GetByIdAsync(existingToken.UserId, ct);
            if (user is null)
            {
                _logger.Warning("Token refresh failed — user not found {UserId}",
                existingToken.UserId);
                throw new InvalidOperationException("User not found");
            }

            // Revoke old refresh token
            existingToken.Revoke();
            await _refreshTokenRepository.SaveChangesAsync(ct);
            _logger.Information("Old refresh token revoked for user {UserId}", user.Id);

            // Generate new access token
            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var expiresAt = DateTime.UtcNow.AddMinutes(60);

            // 6. Generate new refresh token
            var newRefreshTokenValue = _tokenService.GenerateRefreshToken();
            var newRefreshTokenExpiry = _tokenService.GetRefreshTokenExpiry();

            var newRefreshToken = Domain.Entities.RefreshToken.Create(
                user.Id,
                newRefreshTokenValue,
                newRefreshTokenExpiry);

            await _refreshTokenRepository.AddAsync(newRefreshToken, ct);
            await _refreshTokenRepository.SaveChangesAsync(ct);

            // Build response
            var userDto = new UserDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.PhoneNumber,
                user.Role,
                user.CreatedAt);

            _logger.Information("Token refresh successful for user {UserId} {Email}",
            user.Id, user.Email);

            return new AuthResponseDto(
                newAccessToken,
                newRefreshTokenValue,
                expiresAt,
                userDto);
        }
    }
}
