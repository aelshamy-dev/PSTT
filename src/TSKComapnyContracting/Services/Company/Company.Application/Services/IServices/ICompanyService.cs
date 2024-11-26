

using Company.Application.Dto;

namespace Company.Application.Services.IServices;

public interface ICompanyService
{
    Task<List<CompanyDto>> GetAllCompanies();

     int AddCompany(CompanyDto companyDto);
    void UpdateCompany(int companyId,CompanyDto companyDto);
    void DeleteCompany(int companyId);
}
