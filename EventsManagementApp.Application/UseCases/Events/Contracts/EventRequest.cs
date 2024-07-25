namespace EventsManagementApp.Application.UseCases.Events.Contracts;

public record EventRequest(
    string Name,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    string Location,
    string Category,
    int Capacity,
    string[] ImageUrls);