namespace EventsManagementApp.Application.Common.Exceptions;

public class RemoveImagesFailedException : Exception
{
    public RemoveImagesFailedException(string message) : base(message)
    {
    }
}