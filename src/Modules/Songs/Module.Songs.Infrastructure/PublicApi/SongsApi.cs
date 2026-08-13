

using Module.Songs.PublicApi;

namespace Module.Songs.Infrastructure.PublicApi;

public class SongsApi : ISongsApi
{
    public Task CreatePublisherAsync(Guid publisherId, string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
