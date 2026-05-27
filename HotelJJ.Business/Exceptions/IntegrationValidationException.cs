namespace HotelJJ.Business.Exceptions;

public class IntegrationValidationException : IntegrationBusinessException
{
    public IntegrationValidationException(string code, string message, Exception? innerException = null)
        : base(code, message, innerException)
    {
    }
}
