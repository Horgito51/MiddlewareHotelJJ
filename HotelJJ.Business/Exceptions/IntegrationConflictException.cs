namespace HotelJJ.Business.Exceptions;

public class IntegrationConflictException : Exception
{
    public IntegrationConflictException(string code, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        Code = code;
    }

    public string Code { get; }
}
