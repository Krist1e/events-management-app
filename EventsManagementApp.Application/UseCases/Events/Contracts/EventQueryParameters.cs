using EventsManagementApp.Application.Common.Contracts;

namespace EventsManagementApp.Application.UseCases.Events.Contracts;

public class EventQueryParameters : QueryParameters
{
    public string? Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Category { get; set; }
    public string? Location { get; set; }
    
    public string? OrderBy { get; set; }
}
