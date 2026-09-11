namespace CinemaProj.Models;

public enum ScreeningStatus { Scheduled,Cancelled,Completed }
public class Screening
{

    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventId { get; set; }
    public Event? Event { get; set; }
    public DateTimeOffset StartsAt { get; set; }
    public DateTimeOffset EndsAt { get; set; }
    public ScreeningStatus Status { get; set; }
}