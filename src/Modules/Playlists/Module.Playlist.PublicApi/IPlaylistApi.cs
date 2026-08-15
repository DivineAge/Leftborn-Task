

namespace Module.Playlist.PublicApi;

public interface IPlaylistApi
{
    Task CreateUserAsync(
    Guid userId,
    string firstName,
    string lastName,
    CancellationToken cancellationToken = default);

    Task CreateSongAsync(
    Guid songId,
    Guid publisherId,
    int timeInSeconds,
    string name,
    CancellationToken cancellationToken = default);

    Task UpdateUserAsync(
    Guid userId,
    string firstName,
    string lastName,
    CancellationToken cancellationToken = default);

    Task UpdateSongAsync(
    Guid songId,
    Guid publisherId,
    int timeInSeconds,
    string name,
    CancellationToken cancellationToken = default);

}
