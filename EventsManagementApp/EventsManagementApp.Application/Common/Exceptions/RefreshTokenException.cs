namespace EventsManagementApp.Application.Common.Exceptions;

public class RefreshTokenException : Exception
{
    public RefreshTokenException(string message) : base(message)
    {
    }
}