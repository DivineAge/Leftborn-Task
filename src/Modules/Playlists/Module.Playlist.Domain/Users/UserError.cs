using Test.Common.Domain;

namespace Module.Playlist.Domain.Users;

public class UserError
{

    public static Error NotFound(Guid UserId) => Error.NotFound("User.NotFound", $"The user with the identifier {UserId} not found");

    public static Error EmailAlreadyExists(string Email) => Error.Conflict("User.EmailAlreadyExists", $"The user with the email {Email} already exists");

}