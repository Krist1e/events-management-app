namespace EventsManagementApp.Application.UseCases.Users.Commands.RegisterInEvent;

public class RegisterInEventFailedException : Exception
{
    public RegisterInEventFailedException(string message) : base(message)
    {
    }
}