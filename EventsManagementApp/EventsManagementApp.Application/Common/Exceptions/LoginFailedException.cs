namespace EventsManagementApp.Application.Common.Exceptions;

public class LoginFailedException : Exception
{
    public LoginFailedException(string message) : base(message)
    {
    }
}