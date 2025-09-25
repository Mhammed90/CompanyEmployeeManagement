namespace Entities.Exceptions;

public class EmployeeNotFoundException : Exception
{
    public EmployeeNotFoundException(Guid employeeId)
        : base($"Employee with id: {employeeId} was not found.")
    {
    }
}