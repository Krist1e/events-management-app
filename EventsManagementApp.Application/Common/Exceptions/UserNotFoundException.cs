namespace EventsManagementApp.Application.UseCases.Users.Queries.GetUserById;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string message) : base(message)
    {
    }
}