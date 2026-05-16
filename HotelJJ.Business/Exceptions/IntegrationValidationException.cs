namespace HotelJJ.Business.Exceptions;

public class IntegrationValidationException : IntegrationBusinessException
{
    public IntegrationValidationException(string code, string message)
        : base(code, message)
    {
    }
}
