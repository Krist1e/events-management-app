namespace EventsManagementApp.Application.UseCases.Events.Commands.RemoveImages;

public class RemoveImagesFailedException : Exception
{
    public RemoveImagesFailedException(string message) : base(message)
    {
    }
}