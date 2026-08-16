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
        Environment.SetEnvironmentVariable("ConnectionStrings:Database", _dbcontainer.GetConnectionString());
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

