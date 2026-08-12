
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Module.Users.Application.Abstractions.Data;
using Module.Users.Domain.Users;
using Module.Users.Infrastructure.Database;
using Module.Users.Infrastructure.Users;
using Test.Common.Presentation.Endpoints;

namespace Module.Users.Infrastructure;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        services.AddEndPoints(Presentation.AssemblyReference.Assembly);

        return services;
    }
    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<UsersDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("Database"),
                    npgsqlOptions => npgsqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Users))
            );

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUnitOfWork, UsersDbContext>();

    }

}
