
using Company.Application.Dto;
using Company.Application.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Company.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;
    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }
    [HttpGet]
    public async Task<ActionResult<List<CompanyDto>>> Get()
    {
        return await _companyService.GetAllCompanies();
    }
    [HttpPost]
    public int Post([FromBody] CompanyDto companyDto)
    {
        return  _companyService.AddCompany(companyDto);
    }
}
