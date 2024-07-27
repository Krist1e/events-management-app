namespace EventsManagementApp.Application.UseCases.Events.Contracts;

public record EventResponse(
    Guid Id,
    string Name,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    string Location,
    string Category,
    int Capacity,
    List<string> ImageUrls,
    string UserId
);