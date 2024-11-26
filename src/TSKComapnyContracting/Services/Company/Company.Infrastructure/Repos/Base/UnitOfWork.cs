using Company.Domain.Repos;
using Company.Domain.Repos.Base;
using Company.Infrastructure.Context;

namespace Company.Infrastructure.Repos.Base;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        BranchRepository = new BranchRepository(_dbContext);
        CompanyRepository = new CompanyRepository(_dbContext);
    }

    public IBranchRepository BranchRepository { get; set; }

    public ICompanyRepository CompanyRepository { get; set; }

    public void Save()
    {
        _dbContext.SaveChanges();
    }

    public void ClearContext()
    {
        _dbContext.ChangeTracker.Clear();
    }
}
