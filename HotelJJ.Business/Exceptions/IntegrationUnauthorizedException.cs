namespace HotelJJ.Business.Exceptions;

public class IntegrationUnauthorizedException : IntegrationBusinessException
{
    public IntegrationUnauthorizedException(string code, string message, Exception? innerException = null)
        : base(code, message, innerException)
    {
    }
}
