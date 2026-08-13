using Microsoft.EntityFrameworkCore;
using Module.Songs.Domain.Publisher;
using Module.Songs.Infrastructure.Database;

namespace Module.Songs.Infrastructure.Publishers;

public class PublisherRepository(SongsDbContext dbContext) : IPublisherRepository
{
    public async Task<Publisher?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Publishers.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void Insert(Publisher publisher)
    {
        dbContext.Publishers.Add(publisher);
    }
}
