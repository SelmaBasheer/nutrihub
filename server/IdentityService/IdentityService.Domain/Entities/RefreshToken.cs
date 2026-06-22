using System;


namespace IdentityService.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; private set; }
        public string Token { get; private set; } = string.Empty;
        public Guid UserId { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool IsRevoked { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private RefreshToken() { }

        public static RefreshToken Create(Guid userId, string token, DateTime expiresAt)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty");

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be empty");

            return new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = token,
                UserId = userId,
                ExpiresAt = expiresAt,
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void Revoke()
        {
            IsRevoked = true;
        }
    }

}
