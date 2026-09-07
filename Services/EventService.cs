using CinemaProj.Data;
using CinemaProj.DTO;
using CinemaProj.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaProj.Services;

public class EventService : IEventService
{
    private readonly AppDbContext _db;
    private readonly ISeatService _seatService;
    public EventService(AppDbContext db,ISeatService seatService)
    {
        _db = db;
        _seatService = seatService;
    }

    public async Task<List<Event>> GetAllAsync()
    {
        return await _db.Events.ToListAsync();
    }

    public async Task<Event> GetByIdAsync(Guid id)
    {
        return await _db.Events.FindAsync(id);
    }

    public async Task<Event> CreateAsync(RegisterEvent dto)
    {
        var newEvent = new Event
        {
            Title = dto.Title,
            Genre = dto.Genre
        };
        _db.Events.Add(newEvent);
        await _db.SaveChangesAsync();
        
        var seats = await _seatService.GenerateLayout(dto.NumbersOfSeatings, newEvent);
        newEvent.Seats = seats;
        
        return newEvent;
    }
}