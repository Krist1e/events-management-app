namespace EventsManagementApp.Application.UseCases.Events.Contracts;

public record CreateEventRequest(
    string Name,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    string Location,
    string Category,
    int Capacity
);