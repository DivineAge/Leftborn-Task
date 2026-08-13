
namespace Module.Songs.PublicApi;

public interface ISongsApi
{
    Task CreatePublisherAsync(
    Guid publisherId,
    string firstName,
    string lastName,
    CancellationToken cancellationToken = default);
}

