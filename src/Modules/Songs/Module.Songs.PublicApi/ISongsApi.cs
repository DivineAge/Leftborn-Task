
namespace Module.Songs.PublicApi;

public interface ISongsApi
{
    Task CreatePublisherAsync(
    Guid publisherId,
    string firstName,
    string lastName,
    string email,
    CancellationToken cancellationToken = default);

    Task UpdatePublisherAsync(
    Guid publisherId,
    string firstName,
    string lastName,
    CancellationToken cancellationToken = default);
    Task DeletePublisherAsync(
    Guid publisherId,
    CancellationToken cancellationToken = default);
}

