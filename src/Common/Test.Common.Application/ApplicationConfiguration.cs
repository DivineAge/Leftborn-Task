

using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Test.Common.Application.Behaviors;

namespace Test.Common.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services, Assembly[] assemblies)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assemblies);

            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));


        });
        services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);
        return services;
    }

}
