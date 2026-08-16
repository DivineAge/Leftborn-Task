using Test.Common.Domain;

namespace Module.Users.Domain.Users;

public class UserErrors
{
    public static Error NotFound(Guid userId) => Error.NotFound("Users.NotFound", $"The user with the identifier {userId} not found");
    public static Error EmailAlreadyExists(string email) => Error.Conflict("Users.EmailAlreadyExists", $"The user with the email {email} already exists");

}