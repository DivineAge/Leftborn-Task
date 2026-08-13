

using MediatR;
using Module.Songs.PublicApi;
using Module.Users.Application.Users.GetUser;
using Module.Users.Domain.Users;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Users.Application.Users.RegisterUser;

internal sealed class UserRegisteredDomainEventHandler(ISender sender, ISongsApi songsApi)
    : IDomainEventHandler<UserRegisterDomainEvent>
{
    public async Task Handle(UserRegisterDomainEvent notification, CancellationToken cancellationToken)
    {
        Result<UserResponse> result = await sender.Send(new GetUserQuery(notification.UserId), cancellationToken);

        if (result.IsFailure)
        {
            return;
        }
        await songsApi.CreatePublisherAsync(result.Value.Id, result.Value.FirstName, result.Value.LastName, cancellationToken);
    }

}
