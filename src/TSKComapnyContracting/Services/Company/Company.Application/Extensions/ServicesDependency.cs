using Company.Application.Services;
using Company.Application.Services.IServices;
using Company.Domain.Repos.Base;
using Company.Infrastructure.Repos.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Application.Extensions;

public static class ServicesDependency
{
    public static IServiceCollection AddServicesDependency(this IServiceCollection services)
    {
        services.AddScoped(typeof(ICompanyService), typeof(CompanyService));
        return services;
    }
}
