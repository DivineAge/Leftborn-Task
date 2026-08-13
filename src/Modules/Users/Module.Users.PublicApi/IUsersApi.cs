
namespace Module.Users.PublicApi;

public interface IUsersApi
{
    Task<UserResponse?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default);
}

public sealed record UserResponse(Guid Id, string FirstName, string LastName);