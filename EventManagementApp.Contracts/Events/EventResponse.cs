namespace EventManagementApp.Contracts.Events;

public record EventResponse(
    Guid Id,
    string Name,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    string Location,
    string Category,
    int Capacity,
    string[] ImageUrls
);