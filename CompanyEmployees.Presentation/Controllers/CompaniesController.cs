using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _servicemanager;

    public CompaniesController(IServiceManager servicemanager)
    {
        this._servicemanager = servicemanager;
    }

    [HttpGet]
    public IActionResult GetAllCompanies(bool trackChanges)
    {
        try
        {
            var companies = _servicemanager.CompanyService.GetAllCompanies(trackChanges:false);
            return Ok(companies);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}