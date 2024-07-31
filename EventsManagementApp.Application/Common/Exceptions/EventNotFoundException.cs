namespace EventsManagementApp.Application.UseCases.Events.Commands.UpdateEvent;

public class EventNotFoundException : Exception
{
    public EventNotFoundException(string message) : base(message)
    {
    }
}