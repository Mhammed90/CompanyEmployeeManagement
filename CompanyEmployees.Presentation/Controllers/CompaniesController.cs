using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ModelBinders;
using Entities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public CompaniesController(IServiceManager serviceManager)
    {
        this._serviceManager = serviceManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCompanies(bool trackChanges)
    {
        var companies = await _serviceManager.CompanyService.GetAllCompaniesAsync(trackChanges: false);
        return Ok(companies);
    }

    [HttpGet("{companyId:guid}", Name = "CompanyById")]
    public async Task<IActionResult> GetCompanyById(Guid companyId)
    {
        var companies = await _serviceManager.CompanyService.GetCompanyAsync(companyId, trackChanges: true);
        if (companies is null)
            throw new CompanyNotFoundException(companyId);
        return Ok(companies);
    }


    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
    {
        var createdCompany = await _serviceManager.CompanyService.CreateCompanyAsync(company);
        return CreatedAtRoute("CompanyById", new { CompanyId = createdCompany.Id },
            createdCompany);
    }

    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType =
            typeof(ArrayModelBinder))]
        IEnumerable<Guid> ids)
    {
        var companies = await _serviceManager.CompanyService.GetByIdsAsync(ids, trackChanges: false);

        return Ok(companies);
    }

    [HttpPost("collection")]
    public async Task<IActionResult> CreateCompanyCollection(
        [FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
    {
        var result =
            await _serviceManager.CompanyService.CreateCompanyCollectionAsync(companyCollection);
        return CreatedAtRoute("CompanyCollection", new { result.ids },
            result.companies);
    }

    [HttpDelete("{companyId:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid companyId)
    {
        await _serviceManager.CompanyService.DeleteCompanyAsync(companyId, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{companyId:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompany(Guid companyId, CompanyForUpdateDto company)
    {
        await _serviceManager.CompanyService.UpdateCompanyAsync(companyId, company, trackChanges: true);
        return NoContent();
    }
}