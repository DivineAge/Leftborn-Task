using Test.Common.Domain;

namespace Module.Users.Domain.Users;

public class UserError
{
    public static Error NotFound(Guid userId) => Error.NotFound("Users.NotFound", $"The user with the identifier {userId} not found");

}