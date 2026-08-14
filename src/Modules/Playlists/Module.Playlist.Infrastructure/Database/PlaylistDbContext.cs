
using Module.Playlist.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Module.Playlist.Application.Abstractions.Data;
using Module.Playlist.Infrastructure.Users;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;
using Module.Playlist.Domain.Songs;
using Module.Playlist.Infrastructure.Playlists;
using Module.Playlist.Infrastructure.Songs;
using Module.Playlist.Domain.PlaylistSongs;
using Module.Playlist.Infrastructure.PlaylistSongs;


namespace Module.Playlist.Infrastructure.Database;

public sealed class PlaylistDbContext(DbContextOptions<PlaylistDbContext> options) : DbContext(options), IUnitOfWork
{

    internal DbSet<User> Users { get; set; }
    internal DbSet<Song> Songs { get; set; }
    internal DbSet<Domain.Playlists.Playlist> Playlists { get; set; }
    internal DbSet<PlaylistSong> PlaylistSongs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Playlist);


        modelBuilder.ApplyConfiguration(new UserConfiguration());

        modelBuilder.ApplyConfiguration(new SongConfiguration());

        modelBuilder.ApplyConfiguration(new PlaylistConfiguration());

        modelBuilder.ApplyConfiguration(new PlaylistSongConfiguration());
    }

    public async Task<DbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction is not null)
        {
            await Database.CurrentTransaction.DisposeAsync();
        }

        return (await Database.BeginTransactionAsync(cancellationToken)).GetDbTransaction();
    }
}
