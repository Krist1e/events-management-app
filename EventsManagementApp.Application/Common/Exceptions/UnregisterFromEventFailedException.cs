namespace EventsManagementApp.Application.Common.Exceptions;

public class UnregisterFromEventFailedException : Exception
{
    public UnregisterFromEventFailedException(string message) : base(message)
    {
    }
}