using Company.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Infrastructure.Dependencies
{
    public static class DatabaseConfigurationExtension
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
}
