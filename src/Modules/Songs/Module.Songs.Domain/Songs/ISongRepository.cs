
namespace Module.Songs.Domain.Songs;

public interface ISongRepository
{
    Task<Song?> GetAsync(Guid songId, CancellationToken cancellationToken = default);

    void Insert(Song song);

    void Delete(Guid songId);

}
