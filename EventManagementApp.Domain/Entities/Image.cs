namespace EventManagementApp.Domain.Entities;

public class Image
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; }
    public string ImageStorageName { get; set; }
    public Event Event { get; set; }
    public Guid EventId { get; set; }
}