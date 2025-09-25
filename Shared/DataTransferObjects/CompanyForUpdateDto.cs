namespace Shared.DataTransferObjects;

public record CompanyForUpdateDto
{
    public string? Name { get; init; }
    public string? Address { get; init; }
    public string? Country { get; init; }
    IEnumerable<EmployeeForCreationDto> Employees { get; init; }
}