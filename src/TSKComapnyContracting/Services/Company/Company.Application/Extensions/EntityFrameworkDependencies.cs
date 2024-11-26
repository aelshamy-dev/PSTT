using Company.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Application.Extensions;

public static class EntityFrameworkDependencies
{
    public static IServiceCollection AddEntityFrameworkDependencies(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString,
                sqlOptions =>
                {
                    sqlOptions.CommandTimeout(300);
                    sqlOptions.EnableRetryOnFailure(5);
                });
        });

        return services;
    }
}
