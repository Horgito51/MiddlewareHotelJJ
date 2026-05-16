namespace HotelJJ.Business.Exceptions;

public class IntegrationBusinessException : Exception
{
    public IntegrationBusinessException(string code, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        Code = code;
    }

    public string Code { get; }
}
