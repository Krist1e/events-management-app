namespace EventsManagementApp.Application.UseCases.Events.Contracts;

public record EventResponse(
    string Id,
    string Name,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    string Location,
    string Category,
    int Capacity,
    IEnumerable<ImageResponse> Images
    );