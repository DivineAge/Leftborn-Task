
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Playlist.Application.Abstractions.Data;
using Module.Playlist.Domain.PlaylistSongs;
using Module.Playlist.Domain.Playlists;
using Module.Playlist.Domain.Songs;
using Module.Playlist.Domain.Users;
using Module.Playlist.Infrastructure.Database;
using Module.Playlist.Infrastructure.Playlists;
using Module.Playlist.Infrastructure.PlaylistSongs;
using Module.Playlist.Infrastructure.Songs;
using Module.Playlist.Infrastructure.Users;
using Test.Common.Presentation.Endpoints;
using Module.Playlist.PublicApi;
using Module.Playlist.Infrastructure.PublicApi;

namespace Module.Playlist.Infrastructure;

public static class PlaylistModule
{
    public static IServiceCollection AddPlaylistModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        services.AddEndPoints(Presentation.AssemblyReference.Assembly);

        return services;
    }
    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {


        services.AddDbContext<PlaylistDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("Database"),
                npgsqlOptions => npgsqlOptions
                    .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Playlist)));

        services.AddScoped<ISongRepository, SongRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPlaylistRepository, PlaylistRepository>();
        services.AddScoped<IPlaylistSongsRepository, PlaylistSongRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<PlaylistDbContext>());

        services.AddScoped<IPlaylistApi, PlaylistApi>();
    }

}
