using IdentityService.Domain.Entities;
using IdentityService.Domain.Enums;
using Microsoft.EntityFrameworkCore;


namespace IdentityService.Infrastructure.Persistence
{
    public class DataSeeder
    {
        private readonly IdentityDbContext _context;

        public DataSeeder(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Apply any pending migrations automatically
            await _context.Database.MigrateAsync();

            // Seed Admin user
            await SeedAdminAsync();
        }

        private async Task SeedAdminAsync()
        {
            // Check if admin already exists
            var adminExists = await _context.Users
                .AnyAsync(u => u.Email == "admin@nutrihub.com");

            if (adminExists)
                return;  // skip — already seeded

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");

            // Create admin user
            var admin = User.Create(
                firstName: "Admin",
                lastName: "NutriHub",
                email: "admin@nutrihub.com",
                passwordHash: passwordHash,
                phoneNumber: "0000000000",
                role: UserRole.Admin);

            await _context.Users.AddAsync(admin);
            await _context.SaveChangesAsync();

            Console.WriteLine("Admin user seeded successfully!");
        }
    }
}
