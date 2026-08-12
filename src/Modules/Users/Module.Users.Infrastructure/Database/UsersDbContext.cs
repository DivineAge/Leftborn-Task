using Microsoft.EntityFrameworkCore;
using Module.Users.Application.Abstractions.Data;
using Module.Users.Domain.Users;
using Module.Users.Infrastructure.Users;


namespace Module.Users.Infrastructure.Database;

public class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options), IUnitOfWork
{
    internal DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Users);

        modelBuilder.ApplyConfiguration(new UserConfiguration());


    }


}
