using MediatR;
using Module.Songs.Application.Publisher.CreatePublisher;
using Module.Songs.Application.Publisher.DeletePublisher;
using Module.Songs.Application.Publisher.UpdatePublisher;
using Module.Songs.PublicApi;

namespace Module.Songs.Infrastructure.PublicApi;

internal class SongsApi(ISender sender) : ISongsApi
{
    public async Task CreatePublisherAsync(Guid publisherId, string firstName, string lastName, string email, CancellationToken cancellationToken = default)
    {

        await sender.Send(new CreatePublisherCommand(publisherId, firstName, lastName, email), cancellationToken);

    }

    public async Task DeletePublisherAsync(Guid publisherId, CancellationToken cancellationToken = default)
    {

        await sender.Send(new DeletePublisherCommand(publisherId), cancellationToken);

    }

    public async Task UpdatePublisherAsync(Guid publisherId, string firstName, string lastName, CancellationToken cancellationToken = default)
    {

        await sender.Send(new UpdatePublisherCommand(publisherId, firstName, lastName), cancellationToken);

    }
}
