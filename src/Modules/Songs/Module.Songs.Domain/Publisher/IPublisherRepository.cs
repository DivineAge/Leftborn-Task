namespace Module.Songs.Domain.Publisher;

public interface IPublisherRepository
{
    Task<Publisher?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    void Insert(Publisher publisher);

}
