namespace Entities.Exceptions;

public sealed class IdParametersBadRequestException : BadRequestException
{
    public IdParametersBadRequestException() 
        : base("Prameter Ids are null.")
    {
    }
}