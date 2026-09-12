namespace CinemaProj.Models;

public class Seat
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string SeatNumber { get; set; } = string.Empty;
    public Guid RoomId { get; set; } 
    public Room? Room { get; set; }
}