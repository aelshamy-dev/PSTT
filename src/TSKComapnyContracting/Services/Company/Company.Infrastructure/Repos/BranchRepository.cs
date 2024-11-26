

using Company.Domain.Models;
using Company.Domain.Repos;
using Company.Infrastructure.Context;
using Company.Infrastructure.Repos.Base;

namespace Company.Infrastructure.Repos;

public class BranchRepository : BaseRepository<Branch>, IBranchRepository
{
    private AppDbContext _dbContext;

    public BranchRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
