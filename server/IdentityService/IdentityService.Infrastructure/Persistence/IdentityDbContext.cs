using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace IdentityService.Infrastructure.Persistence
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

                entity.Property(u => u.PasswordHash)
                .IsRequired();

                entity.Property(u => u.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

                entity.Property(u => u.Role)
                .IsRequired()
                .HasConversion<string>();

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                // Address — stored as owned entity (same table, no separate table)
                entity.OwnsOne(u => u.Address, address =>
                {
                    address.Property(a => a.Street).HasMaxLength(200);
                    address.Property(a => a.City).HasMaxLength(100);
                    address.Property(a => a.State).HasMaxLength(100);
                    address.Property(a => a.PostalCode).HasMaxLength(20);
                    address.Property(a => a.Country).HasMaxLength(100);
                });
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Token)
                    .IsRequired();

                entity.HasIndex(r => r.Token)
                    .IsUnique();

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
