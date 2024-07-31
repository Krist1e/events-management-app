namespace EventsManagementApp.Application.UseCases.Events.Contracts;

public record EventRequest(
    string Id,
    string Name,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    string Location,
    string Category,
    int Capacity
);