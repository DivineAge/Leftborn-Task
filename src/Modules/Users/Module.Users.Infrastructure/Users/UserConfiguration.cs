
using Microsoft.EntityFrameworkCore;
using Module.Users.Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Module.Users.Infrastructure.Users;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName).HasMaxLength(200);

        builder.Property(u => u.LastName).HasMaxLength(200);

        builder.Property(u => u.Email).HasMaxLength(200);

        // email should be unique 
        builder.HasIndex(u => u.Email).IsUnique();



    }

}
