namespace EventsManagementApp.Application.Common.Exceptions;

public class RegisterFailedException : Exception
{
    public RegisterFailedException(string message) : base(message)
    {
    }
}