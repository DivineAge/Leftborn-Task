
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Module.Playlist.Domain.Playlists;
using Module.Playlist.Domain.Users;

namespace Module.Playlist.Infrastructure.Playlists;

internal sealed class PlaylistConfiguration : IEntityTypeConfiguration<Domain.Playlists.Playlist>
{
    public void Configure(EntityTypeBuilder<Domain.Playlists.Playlist> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.OwnerId)
            .IsRequired();

        builder.HasOne<User>().WithMany().HasForeignKey(x => x.OwnerId).HasPrincipalKey(p => p.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);
    }

}
