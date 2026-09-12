namespace CinemaProj.Models;

public class Room
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public int SeatsNumber { get; set; }
    public List<Seat> Seats { get; set; } = new();
    public List<Screening> Screenings { get; set; } = new();
}
