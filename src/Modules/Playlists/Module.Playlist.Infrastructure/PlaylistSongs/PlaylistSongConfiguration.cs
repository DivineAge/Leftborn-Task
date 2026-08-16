using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Module.Playlist.Domain.PlaylistSongs;
using Module.Playlist.Domain.Songs;

namespace Module.Playlist.Infrastructure.PlaylistSongs;

internal sealed class PlaylistSongConfiguration : IEntityTypeConfiguration<PlaylistSong>
{
    public void Configure(EntityTypeBuilder<PlaylistSong> builder)
    {
        builder.HasKey(x => new { x.PlaylistId, x.SongId });

        builder.HasOne<Domain.Playlists.Playlist>().WithMany().HasForeignKey(x => x.PlaylistId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Song>().WithMany().HasForeignKey(x => x.SongId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.PlaylistId)
            .IsRequired();

        builder.Property(x => x.SongId)
            .IsRequired();

    }
}
