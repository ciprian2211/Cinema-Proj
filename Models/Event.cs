namespace CinemaProj.Models;

public class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int SeatsNumber { get; set; }
    public List<Seat> SeatTemplate { get; set; } = new();
    public List<Screening> Screenings { get; set; } = new();
}