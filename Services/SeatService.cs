using CinemaProj.Data;
using CinemaProj.Models;

namespace CinemaProj.Services;

public class SeatService : ISeatService
{
    private readonly AppDbContext _db;

    public SeatService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Seat>> GenerateLayout(int seatsNumber, Event registeredEvent)
    {
        int columns = seatsNumber / 2 == 0 ? 1 : seatsNumber / 2;
        int rows = seatsNumber % columns != 0 ? seatsNumber / columns + 1 : seatsNumber / columns;

        List<Seat> seatings = new List<Seat>();
        int seatCounter = 0;
        
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (seatCounter >= seatsNumber)
                {
                    break;
                }

                char rowLetter = (char)(i + 65);
                int columnNumber = j + 1;
                var seat = new Seat
                {
                    SeatNumber = $"{rowLetter}{columnNumber}",
                    Event = registeredEvent,
                    EventId = registeredEvent.Id
                };
                 _db.Seats.Add(seat);
                seatings.Add(seat);
                seatCounter++;
            }
        }

        await _db.SaveChangesAsync();
        return seatings;
    }
}