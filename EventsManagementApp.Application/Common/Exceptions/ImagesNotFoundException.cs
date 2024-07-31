namespace EventsManagementApp.Application.Common.Exceptions;

public class ImagesNotFoundException : Exception
{
    public ImagesNotFoundException(string message) : base(message)
    {
    }
}