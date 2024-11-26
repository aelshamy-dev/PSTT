

namespace Company.Domain.Repos.Base;

public interface IUnitOfWork
{
    IBranchRepository BranchRepository { get; }
    ICompanyRepository CompanyRepository { get; }
    void ClearContext();
    void Save();
}
