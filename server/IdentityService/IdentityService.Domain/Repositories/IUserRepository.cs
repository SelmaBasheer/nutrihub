using IdentityService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id, CancellationToken ct=default);
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task<bool> IsUserExistsAsync(string email, CancellationToken ct = default);
        Task AddAsync(User user, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
