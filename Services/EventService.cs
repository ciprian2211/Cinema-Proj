using CinemaProj.Data;
using CinemaProj.DTO;
using CinemaProj.Exceptions;
using CinemaProj.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
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

    public async Task<List<EventListDto>> GetAllAsync()
    {
        return await _db.Events.AsNoTracking().Select(e => 
            new EventListDto(
                    e.Id,
                    e.Title,
                    e.Genre,
                    e.SeatsNumber,
                    e.Screenings.Count
                )).ToListAsync();
    }

    public async Task<EventDto?> GetByIdAsync(Guid id, CancellationToken ct =default)
    {
        return await _db.Events
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new EventDto(
                e.Id,
                e.Title,
                e.Genre,
                e.SeatsNumber,
                e.SeatTemplate
                    .OrderBy(s => s.SeatNumber)
                    .Select(s => s.SeatNumber)
                    .ToList(),
                e.Screenings
                    .OrderBy(s => s.StartsAt)
                    .Select(s =>
                        new ScreeningDto(s.Id, s.StartsAt, s.EndsAt, s.Status.ToString())).ToList()
            ))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<EventDto> CreateAsync(RegisterEvent dto, CancellationToken ct = default)
    {
        const int minSeats = 10;
        const int maxSeats = 100;
        const int minDuration = 30;
        const int maxDuration = 300;
        const int maxScreenings = 10;

        
        var title = (dto.Title ?? string.Empty).Trim();
        var genre = (dto.Genre ?? string.Empty).Trim();
        var screenings = dto.Screenings ?? new List<RegisterScreening>();

        if (string.IsNullOrWhiteSpace(title) || title.Length > 150)
            throw new ValidationException("event.title", "Title must be 1..150 chars.");

        if (string.IsNullOrWhiteSpace(genre) || genre.Length > 100)
            throw new ValidationException("event.genre", "Genre must be 1..100 chars.");

        if (dto.SeatsNumber < minSeats || dto.SeatsNumber > maxSeats)
            throw new ValidationException("event.seats", $"Seats must be between {minSeats} and {maxSeats}.");

        if (dto.DurationInMinutes < minDuration || dto.DurationInMinutes > maxDuration)
            throw new ValidationException("event.duration", $"Duration must be between {minDuration} and {maxDuration}.");

        if (dto.Screenings.Count > maxScreenings)
            throw new ValidationException("event.screenings", $"Max {maxScreenings} screenings per create.");

        var startsUtc = dto.Screenings.Select(s => s.StartsAt.ToUniversalTime()).ToList();

        if (startsUtc.Any(s => s <= DateTimeOffset.UtcNow))
            throw new ValidationException("screening.startsAt", "StartsAt must be future UTC.");

        if (startsUtc.Distinct().Count() != startsUtc.Count)
            throw new DuplicateScreeningException("Duplicate StartsAt in request.");

    
        var newEvent = new Event
        {
            Title = title,
            Genre = genre,
            SeatsNumber = dto.SeatsNumber
        };

        newEvent.SeatTemplate = _seatService.GenerateLayout(dto.SeatsNumber, newEvent.Id);

        newEvent.Screenings = startsUtc.Select(s => new Screening
        {
            Id = Guid.NewGuid(),
            EventId = newEvent.Id,
            StartsAt = s,
            EndsAt = s.AddMinutes(dto.DurationInMinutes),
            Status = ScreeningStatus.Scheduled
        }).ToList();

      
        _db.Events.Add(newEvent);
        try
        {
            await _db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg && pg.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new DuplicateScreeningException("Same StartsAt already exists.");
        }

        return new EventDto(
            newEvent.Id,
            newEvent.Title,
            newEvent.Genre,
            newEvent.SeatsNumber,
            newEvent.SeatTemplate.Select(s => s.SeatNumber).ToList(),
            newEvent.Screenings.Select(s => new ScreeningDto(s.Id, s.StartsAt, s.EndsAt, s.Status.ToString())).ToList()
        );

    }
}