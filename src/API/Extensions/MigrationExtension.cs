using Microsoft.EntityFrameworkCore;
using Module.Users.Infrastructure;
using Module.Users.Infrastructure.Database;
using Module.Songs.Infrastructure;
using Module.Songs.Infrastructure.Database;

namespace API.Extensions;

public static class MigrationExtension
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        ApplyMigration<UsersDbContext>(scope);
        ApplyMigration<SongsDbContext>(scope);
    }
    private static void ApplyMigration<TDbContext>(IServiceScope scope)
    where TDbContext : DbContext
    {
        using TDbContext context = scope.ServiceProvider.GetRequiredService<TDbContext>();

        context.Database.Migrate();
    }
}
