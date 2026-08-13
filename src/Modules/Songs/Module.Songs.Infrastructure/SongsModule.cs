
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Songs.Application.Abstractions.Data;
using Module.Songs.Domain.Publisher;
using Module.Songs.Domain.Songs;
using Module.Songs.Infrastructure.Database;
using Module.Songs.Infrastructure.PublicApi;
using Module.Songs.Infrastructure.Publishers;
using Module.Songs.Infrastructure.Songs;
using Module.Songs.PublicApi;
using Test.Common.Presentation.Endpoints;

namespace Module.Songs.Infrastructure;

public static class SongsModule
{
    public static IServiceCollection AddSongsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        services.AddEndPoints(Presentation.AssemblyReference.Assembly);

        return services;
    }
    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<ISongsApi, SongsApi>();
        services.AddDbContext<SongsDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("Database"),
                npgsqlOptions => npgsqlOptions
                    .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Songs)));

        services.AddScoped<ISongRepository, SongRepository>();
        services.AddScoped<IPublisherRepository, PublisherRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<SongsDbContext>());

        services.AddScoped<ISongsApi, SongsApi>();
    }

}
