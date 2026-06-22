

using IdentityService.Domain.Enums;

namespace IdentityService.Application.DTOs
{
    public record UserDto(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        UserRole Role,
        DateTime CreatedAt
    );
}
