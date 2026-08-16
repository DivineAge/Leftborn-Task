using Microsoft.EntityFrameworkCore;
using Module.Users.Application.Abstractions.Data;
using Module.Users.Domain.Users;
using Module.Users.Infrastructure.Users;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;
namespace Module.Users.Infrastructure.Database;

public class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options), IUnitOfWork
{
    internal DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Users);

        modelBuilder.ApplyConfiguration(new UserConfiguration());


    }
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction is not null)
        {
            await Database.CurrentTransaction.DisposeAsync();
        }

        await Database.BeginTransactionAsync(cancellationToken);
        
    }
    
}
