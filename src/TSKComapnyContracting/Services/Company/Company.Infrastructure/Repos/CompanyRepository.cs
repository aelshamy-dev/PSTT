

using Company.Domain.Repos;
using Company.Infrastructure.Context;
using Company.Infrastructure.Repos.Base;

namespace Company.Infrastructure.Repos;

public class CompanyRepository:BaseRepository<Company.Domain.Models.Company>,ICompanyRepository
{
    private AppDbContext _dbContext;

    public CompanyRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
