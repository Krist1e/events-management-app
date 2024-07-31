namespace EventsManagementApp.Application.UseCases.Events.Commands.RemoveImages;

public class ImagesNotFoundException : Exception
{
    public ImagesNotFoundException(string message) : base(message)
    {
    }
}