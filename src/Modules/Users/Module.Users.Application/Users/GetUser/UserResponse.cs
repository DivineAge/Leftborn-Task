
namespace Module.Users.Application.Users.GetUser;

public sealed record UserResponse(Guid Id, string FirstName, string LastName, string Email);

