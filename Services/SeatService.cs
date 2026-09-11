using CinemaProj.Data;
using CinemaProj.Models;

namespace CinemaProj.Services;

public class SeatService : ISeatService
{
    
    public  List<Seat> GenerateLayout(int seatsNumber, Guid eventId)
    {
        int columns = Math.Min(10, seatsNumber);
        int rows = (int)Math.Ceiling(seatsNumber / (double)columns);

        var seats = new List<Seat>(seatsNumber);
        int counter = 0;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 1; c <= columns; c++)
            {
                if (counter >= seatsNumber) break;
                seats.Add(new Seat
                {
                    Id = Guid.NewGuid(),
                    EventId = eventId,
                    SeatNumber = $"{(char)('A' +r)}{c}"
                });
                counter++;
            }
        }

        return seats;
    }
}