namespace EventsManagementApp.Application.Common.Exceptions;

public class RegisterInEventFailedException : Exception
{
    public RegisterInEventFailedException(string message) : base(message)
    {
    }
}