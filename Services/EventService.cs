using CinemaProj.Data;
using CinemaProj.DTO;
using CinemaProj.Exceptions;
using CinemaProj.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaProj.Services;

public class EventService : IEventService
{
    private readonly AppDbContext _db;

    public EventService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<List<EventListDto>>> GetAllAsync()
    {
        var list = await _db.Events.AsNoTracking().Select(e => 
            new EventListDto(
                e.Id,
                e.Title,
                e.Genre,
                e.Screenings.Count
            )).ToListAsync();
            
        return Result<List<EventListDto>>.Success(list);
    }

    public async Task<Result<EventDto?>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var e = await _db.Events
            .AsNoTracking()
            .Where(ev => ev.Id == id)
            .Select(ev => new EventDto(
                ev.Id,
                ev.Title,
                ev.Genre,
                ev.Screenings
                    .OrderBy(s => s.StartsAt)
                    .Select(s => new ScreeningDto(s.Id, s.StartsAt, s.EndsAt, s.Status.ToString(), s.RoomId)).ToList()
            ))
            .FirstOrDefaultAsync(ct);

        if (e is null) return Result<EventDto?>.Failure($"Event with ID {id} not found.");
        
        return Result<EventDto?>.Success(e);
    }

    public async Task<Result<EventDto>> CreateAsync(RegisterEvent dto, CancellationToken ct = default)
    {
        var title = (dto.Title ?? string.Empty).Trim();
        var genre = (dto.Genre ?? string.Empty).Trim();

        
        if (string.IsNullOrWhiteSpace(title) || title.Length > 150)
            return Result<EventDto>.Failure("Title must be 1..150 chars.");

        if (string.IsNullOrWhiteSpace(genre) || genre.Length > 100)
            return Result<EventDto>.Failure("Genre must be 1..100 chars.");

        var newEvent = new Event
        {
            Title = title,
            Genre = genre
        };

        _db.Events.Add(newEvent);
        await _db.SaveChangesAsync(ct);

        var eventDto = new EventDto(
            newEvent.Id,
            newEvent.Title,
            newEvent.Genre,
            new List<ScreeningDto>()
        );

        return Result<EventDto>.Success(eventDto);
    }
}