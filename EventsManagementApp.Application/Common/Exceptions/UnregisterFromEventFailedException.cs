namespace EventsManagementApp.Application.UseCases.Users.Commands.UnregisterFromEvent;

public class UnregisterFromEventFailedException : Exception
{
    public UnregisterFromEventFailedException(string message) : base(message)
    {
    }
}