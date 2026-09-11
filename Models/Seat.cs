namespace CinemaProj.Models;

public class Seat
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string SeatNumber { get; set; } = string.Empty;
    public Guid EventId { get; set; } 
    public Event? Event { get; set; }
}