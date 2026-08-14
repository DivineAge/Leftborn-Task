

using Microsoft.EntityFrameworkCore;
using Module.Playlist.Domain.Users;
using Module.Playlist.Domain.Songs;

namespace Module.Playlist.Infrastructure.Songs;

internal sealed class SongConfiguration : IEntityTypeConfiguration<Song>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Song> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PublisherId)
            .IsRequired();

        builder.HasOne<User>().WithMany().HasForeignKey(x => x.PublisherId).HasPrincipalKey(p => p.Id);

        builder.Property(x => x.TimeInSeconds)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);
    }
}
