using CinemaProj.Models;

namespace CinemaProj.Services;

public interface ISeatService
{
    Task<List<Seat>> GenerateLayout(int seatsNumber, Event registeredEvent);
}