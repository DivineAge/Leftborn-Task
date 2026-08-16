using Microsoft.EntityFrameworkCore;
using Module.Songs.Domain.Publisher;
using Module.Songs.Infrastructure.Database;

namespace Module.Songs.Infrastructure.Publishers;

public class PublisherRepository(SongsDbContext dbContext) : IPublisherRepository
{
    public void Delete(Publisher publisher)
    {
        dbContext.Publishers.Remove(publisher);
    }

    public async Task<Publisher?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Publishers.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Publisher?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Publishers.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public void Insert(Publisher publisher)
    {
        dbContext.Publishers.Add(publisher);
    }
}
