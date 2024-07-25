namespace EventManagementApp.Domain.Entities;

public class Image
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; }
    public string ImageStorageName { get; set; }
    
    public ICollection<EventImage> EventImages { get; set; }
}