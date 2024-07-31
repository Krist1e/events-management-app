namespace EventsManagementApp.Application.Common.Exceptions;

public class EventNotFoundException : Exception
{
    public EventNotFoundException(string message) : base(message)
    {
    }
}