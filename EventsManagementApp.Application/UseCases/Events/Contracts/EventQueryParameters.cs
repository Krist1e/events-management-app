using EventsManagementApp.Application.Common.Contracts;

namespace EventsManagementApp.Application.UseCases.Events.Contracts;

public class EventQueryParameters : QueryParameters
{
    public string? Name { get; }
    public DateTime? StartDate { get; }
    public DateTime? EndDate { get; }
    public string? Category { get; }
    public string? Location { get; }
    
    public string? OrderBy { get; }
}
