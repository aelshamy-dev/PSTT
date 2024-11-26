
using Company.Application.Dto;
using Company.Application.Services.IServices;
using Company.Domain.Repos.Base;
using Mapster;



namespace Company.Application.Services;

public class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;
    public CompanyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public int AddCompany(CompanyDto companyDto)
    {
        Domain.Models.Company company = companyDto.Adapt<Domain.Models.Company>();
        _unitOfWork.CompanyRepository.Add(company);
        _unitOfWork.Save();
        return company.CompanyId;
    }

    public void DeleteCompany(int companyId)
    {
        Domain.Models.Company company = _unitOfWork.CompanyRepository.GetById(companyId);
        if (company != null)
        {
            company.IsDeleted = true;
            _unitOfWork.CompanyRepository.Update(company);
            _unitOfWork.Save();
        }
    }

    public async Task<List<CompanyDto>> GetAllCompanies()
    {
        IEnumerable<Domain.Models.Company> companies = await _unitOfWork.CompanyRepository.GetQuery(null, inc => inc.Branch);
        List<CompanyDto> result = companies.Adapt<List<CompanyDto>>();
        return  result;
    }

    public void UpdateCompany(int companyId, CompanyDto companyDto)
    {
        Domain.Models.Company company = _unitOfWork.CompanyRepository.GetById(companyId);
        if (company != null)
        {
            company = companyDto.Adapt<Domain.Models.Company>();
            _unitOfWork.CompanyRepository.Update(company);
            _unitOfWork.Save();
        }
    }
}
