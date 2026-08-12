using API.Extensions;
using API.Middleware;
using Module.Users.Infrastructure;
using System.Reflection;
using Test.Common.Application;
using Test.Common.Infrastructure;
using Test.Common.Infrastructure.Configuration;
using Test.Common.Presentation.Endpoints;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();


Assembly[] moduleAssemblies = [
    Module.Users.Application.AssemblyRefrence.Assembly
];

builder.Services.AddApplication(moduleAssemblies);

string databaseConnectionString = builder.Configuration.GetConnectionStringOrThrow("Database");

builder.Services.AddInfrastructure(databaseConnectionString);

builder.Configuration.AddModuleConfiguration(["users", "events", "ticketing", "attendance"]);

builder.Services.AddUsersModule(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.UseExceptionHandler();

app.MapEndpoints();

app.Run();



