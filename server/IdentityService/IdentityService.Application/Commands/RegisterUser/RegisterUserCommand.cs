using MediatR;


namespace IdentityService.Application.Commands.RegisterUser
{
    public record RegisterUserCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string PhoneNumber
        ) : IRequest<Guid>;

    //record → immutable, values never change after creation
    //IRequest<Guid> → tells MediatR this command returns a Guid (new user's ID)
}
