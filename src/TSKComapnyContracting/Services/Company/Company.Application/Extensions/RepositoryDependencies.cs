using Company.Domain.Repos.Base;
using Company.Infrastructure.Repos.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Application.Extensions;

public static class RepositoryDependencies
{
    public static IServiceCollection AddGenericRepositoryDependencies(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

        return services;
    }

}
