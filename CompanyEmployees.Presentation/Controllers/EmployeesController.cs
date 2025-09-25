using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[ApiController]
[Route("api/companies/{companyId}/employees")]
public class EmployeesController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public EmployeesController(IServiceManager serviceManager)
        => _serviceManager = serviceManager;

    [HttpGet(Name = "GetEmployeeForCompany")]
    public async Task<IActionResult> GetEmployeesForCompany(Guid companyId)
    {
        var employees = await _serviceManager.EmployeeService.GetEmployeesAsync(companyId, trackChanges: false);
        return Ok(employees);
    }

    [HttpGet("{employeeId:guid}")]
    public async Task<IActionResult> GetEmployee(Guid companyId, Guid employeeId)
    {
        var employee =
            await _serviceManager.EmployeeService.GetEmployeeAsync(companyId, employeeId, trackChanges: false);

        return Ok(employee);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId,
        [FromBody] EmployeeForCreationDto employee)
    {
        var employeeToReturn =
            await _serviceManager.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee, trackChanges:
                false);

        return CreatedAtRoute("GetEmployeeForCompany", new
            {
                companyId, id =
                    employeeToReturn.Id
            },
            employeeToReturn);
    }

    [HttpDelete("{employeeId:guid}")]
    public async Task<IActionResult> DeleteEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        await _serviceManager.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, employeeId, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{employeeId:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid employeeId,
        [FromBody] EmployeeForUpdateDto employeeForUpdate)
    {
        await _serviceManager.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, employeeId, employeeForUpdate,
            compTrackChanges: false, empTrackChanges: true);
        return NoContent();
    }
}