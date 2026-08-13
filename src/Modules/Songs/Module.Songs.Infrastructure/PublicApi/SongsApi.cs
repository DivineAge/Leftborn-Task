using MediatR;
using Module.Songs.Application.Publisher.CreatePubliser;
using Module.Songs.PublicApi;
using static MassTransit.ValidationResultExtensions;

namespace Module.Songs.Infrastructure.PublicApi;

internal class SongsApi(ISender sender) : ISongsApi
{
    public async Task CreatePublisherAsync(Guid publisherId, string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        try
        {
            await sender.Send(new CreatePublisherCommand(publisherId, firstName, lastName), cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to create publisher: {ex.Message}", ex);
        }


    }
}
