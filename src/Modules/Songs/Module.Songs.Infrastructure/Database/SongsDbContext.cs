
using Module.Songs.Domain.Publisher;
using Microsoft.EntityFrameworkCore;
using Module.Songs.Application.Abstractions.Data;
using Module.Songs.Infrastructure.Publishers;
using Module.Songs.Domain.Songs;
using Module.Songs.Infrastructure.Songs;

namespace Module.Songs.Infrastructure.Database;

public sealed class SongsDbContext(DbContextOptions<SongsDbContext> options) : DbContext(options), IUnitOfWork
{

    internal DbSet<Publisher> Publishers { get; set; }
    internal DbSet<Song> Songs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Songs);


        modelBuilder.ApplyConfiguration(new PublisherConfiguration());

        modelBuilder.ApplyConfiguration(new SongConfiguration());
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction is not null)
        {
            await Database.CurrentTransaction.DisposeAsync();
        }

        await Database.BeginTransactionAsync(cancellationToken);
        
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction is null)
        {
            return;
        }

        await Database.CurrentTransaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction is null)
        { 
            return;
        }

        await Database.CurrentTransaction.RollbackAsync(cancellationToken);
    }
}
