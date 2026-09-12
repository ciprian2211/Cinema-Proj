using CinemaProj.Models;

namespace CinemaProj.Services;

public interface ISeatService
{
    List<Seat> GenerateLayout(int seatsNumber, Guid roomId);
}