namespace HotelJJ.Business.Exceptions;

public class IntegrationForbiddenException : IntegrationBusinessException
{
    public IntegrationForbiddenException(string code, string message, Exception? innerException = null)
        : base(code, message, innerException)
    {
    }
}
