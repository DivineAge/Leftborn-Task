
using MediatR;
using Module.Users.Application.Users.GetUser;
using Module.Users.PublicApi;
using Test.Common.Domain;
using UserResponse = Module.Users.PublicApi.UserResponse;
namespace Module.Users.Infrastructure.PublicApi;

public class UserApi(ISender sender) : IUsersApi
{
    public async Task<UserResponse?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        Result<Application.Users.GetUser.UserResponse> result = await sender.Send(new GetUserQuery(userId), cancellationToken);

        if (result.IsFailure)
        {
            return null;
        }
        return new UserResponse(result.Value.Id, result.Value.FirstName, result.Value.LastName);
    }

}
