using IdentityService.Domain.Entities;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Repositories;
using MediatR;
using Serilog;


namespace IdentityService.Application.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger = Log.ForContext<RegisterUserCommandHandler>();

        public RegisterUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken ct)
        {
            _logger.Information("Registering new user with email {Email}", request.Email);

            //checks if the user already exists
            var exists = await _userRepository.IsUserExistsAsync(request.Email, ct);
            if (exists)
            {
                _logger.Warning("Registration failed — email already exists {Email}", request.Email);
                throw new InvalidOperationException("User with this email already exists");
            }

            //Hash the password using BCrypt
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            //Create user using factory method
            var user = User.Create(
                request.FirstName,
                request.LastName,
                request.Email,
                passwordHash,
                request.PhoneNumber,
                UserRole.Customer);

            //Save to database
            await _userRepository.AddAsync(user, ct);
            await _userRepository.SaveChangesAsync(ct);

            _logger.Warning("Registration failed — email already exists {Email}", request.Email);

            return user.Id;


        }
    }
}
