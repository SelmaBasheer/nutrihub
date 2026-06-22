using IdentityService.Domain.Enums;
using IdentityService.Domain.ValueObjects;


namespace IdentityService.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set;} = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public UserRole Role { get; private set; }
        public Address? Address { get; private set; }  // optional
        public DateTime CreatedAt { get; private set; }

        private User() { }

        //Factory Method prevents unnecessary user creation
        public static User Create(
            string firstName, 
            string lastName, 
            string email, 
            string passwordHash, 
            string phoneNumber,
            UserRole role)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("First name cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("Last name cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("Password cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty");
            if (!Enum.IsDefined(typeof(UserRole), role))
                throw new ArgumentException("Invalid role");


            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email.Trim().ToLowerInvariant(),
                PasswordHash = passwordHash,
                PhoneNumber = phoneNumber,
                Role = role,
                CreatedAt = DateTime.UtcNow
            };
        }

        // Business method — update address separately after registration
        public void UpdateAddress(Address address)
        {
            Address = address ?? throw new ArgumentNullException(nameof(address));
        }
    }
}
