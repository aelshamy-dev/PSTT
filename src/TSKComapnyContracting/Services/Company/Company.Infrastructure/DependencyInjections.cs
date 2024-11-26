

using Company.Infrastructure.Context;
using Company.Infrastructure.Dependencies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Infrastructure;

public static class DependencyInjections
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFrameworkDependencies(configuration.GetConnectionString("AppConnectionString"));

        return services;
    }
}
