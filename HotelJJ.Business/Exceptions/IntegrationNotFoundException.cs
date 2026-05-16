namespace HotelJJ.Business.Exceptions;

public class IntegrationNotFoundException : IntegrationBusinessException
{
    public IntegrationNotFoundException(string code, string message, Exception? innerException = null)
        : base(code, message, innerException)
    {
    }
}
