namespace CinemaProj.Models;

public class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public List<Seat> Seats { get; set; } = new();
}