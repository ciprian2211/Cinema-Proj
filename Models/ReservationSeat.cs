namespace CinemaProj.Models;

public class ReservationSeat
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ReservationId { get; set; } = Guid.Empty;
    public Reservation? Reservation { get; set; }
    public Guid ScreeningId { get; set; } = Guid.Empty;
    public Screening? Screening { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
}