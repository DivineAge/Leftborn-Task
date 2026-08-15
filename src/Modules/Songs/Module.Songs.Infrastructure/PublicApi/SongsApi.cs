using MediatR;
using Module.Songs.Application.Publisher.CreatePublisher;
using Module.Songs.Application.Publisher.UpdatePublisher;
using Module.Songs.PublicApi;

namespace Module.Songs.Infrastructure.PublicApi;

internal class SongsApi(ISender sender) : ISongsApi
{
    public async Task CreatePublisherAsync(Guid publisherId, string firstName, string lastName, string email, CancellationToken cancellationToken = default)
    {
        try
        {
            await sender.Send(new CreatePublisherCommand(publisherId, firstName, lastName, email), cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to create publisher: {ex.Message}", ex);
        }


    }
    public async Task UpdatePublisherAsync(Guid publisherId, string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        try
        {
            await sender.Send(new UpdatePublisherCommand(publisherId, firstName, lastName), cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to update publisher: {ex.Message}", ex);
        }
    }
}
