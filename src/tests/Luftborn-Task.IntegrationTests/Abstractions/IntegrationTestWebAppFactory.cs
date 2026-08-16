using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace Luftborn_Task.IntegrationTests.Abstractions;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{

    private readonly PostgreSqlContainer _dbcontainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .Build();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        string connectionString = _dbcontainer.GetConnectionString();
        Environment.SetEnvironmentVariable("ConnectionStrings:Database", connectionString);
        Environment.SetEnvironmentVariable("ConnectionStrings__Database", connectionString);
        builder.UseSetting("ConnectionStrings:Database", connectionString);
        builder.ConfigureTestServices(services =>
        {

        });
    }
    public async Task InitializeAsync()
    {
        await _dbcontainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbcontainer.StopAsync();
    }
}

