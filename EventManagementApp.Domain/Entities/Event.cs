using EventManagementApp.Domain.Enums;

namespace EventManagementApp.Domain.Entities;

public class Event
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; }
    public CategoryEnum Category { get; set; }
    public int Capacity { get; set; }
    
    public List<Image> Images { get; set; }
    public ICollection<UserEvent> UserEvents { get; set; }
}