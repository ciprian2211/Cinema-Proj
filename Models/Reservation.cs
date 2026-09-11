namespace CinemaProj.Models;

public enum ReservationStatus {Held,Confirmed,Cancelled,Expired}
public class Reservation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ScreeningId { get; set; }
    public Screening? Screening { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public List<ReservationSeat> Seats { get; set; } = new();
    public ReservationStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    
}