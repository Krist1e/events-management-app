namespace EventsManagementApp.Application.UseCases.Auth.RefreshToken;

public class RefreshTokenException : Exception
{
    public RefreshTokenException(string message) : base(message)
    {
    }
}