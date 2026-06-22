using IdentityService.Domain.Entities;


namespace IdentityService.Domain.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default);
        Task AddAsync(RefreshToken refreshToken, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
