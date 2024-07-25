namespace EventManagementApp.Domain.Entities;

public class EventImage
{
    public Guid EventId { get; set; }
    public Event Event { get; set; }
    
    public Guid ImageId { get; set; }
    public Image Image { get; set; }
}