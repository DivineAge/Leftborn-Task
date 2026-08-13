

using Microsoft.EntityFrameworkCore;
using Module.Songs.Domain.Publisher;
using Module.Songs.Domain.Songs;

namespace Module.Songs.Infrastructure.Songs;

internal sealed class SongConfiguration : IEntityTypeConfiguration<Song>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Song> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PublisherId)
            .IsRequired();

        builder.HasOne<Publisher>().WithMany().HasForeignKey(x => x.PublisherId).HasPrincipalKey(p => p.Id);

        builder.Property(x => x.TimeInSeconds)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);
    }
}
