using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly IdentityDbContext _context;

        public UserRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            await _context.Users.AddAsync(user, ct);
        }

        public async Task<bool> IsUserExistsAsync(string email, CancellationToken ct = default)
        {
            return await _context.Users
             .AnyAsync(u => u.Email == email, ct);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            return await _context.Users
             .FirstOrDefaultAsync(u => u.Email == email, ct);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Users.FindAsync(new object[] { id }, ct);
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
