using Company.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace Company.Infrastructure.Context;
public class AppDbContext:DbContext
{
    public AppDbContext()
    { }
    public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
    { }
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Domain.Models.Company> Companies => Set<Domain.Models.Company>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

}
