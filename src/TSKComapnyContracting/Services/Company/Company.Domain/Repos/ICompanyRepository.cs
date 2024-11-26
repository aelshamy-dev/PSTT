using Company.Domain.Repos.Base;
using mdl=Company.Domain.Models;
namespace Company.Domain.Repos;

public interface ICompanyRepository:IBaseRepository<mdl.Company>
{
}
