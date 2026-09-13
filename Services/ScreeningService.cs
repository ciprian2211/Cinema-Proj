using CinemaProj.Data;
using CinemaProj.DTO;
using CinemaProj.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CinemaProj.Services;

public class ScreeningService : IScreeningService
{
    private readonly AppDbContext _db;
    private readonly IEventService _eventService;
    private readonly IRoomService _roomService;
    public ScreeningService(AppDbContext db,IEventService eventService,IRoomService roomService)
    {
        _db = db;
        _eventService = eventService;
        _roomService = roomService;
    }


    public async Task<Result<ScreeningDto>> CreateAsync(RegisterScreening dto, CancellationToken ct = default)
    {
        var eventTitle = (dto.EventTitle?? string.Empty).Trim();
        var roomName = (dto.RoomName??string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(eventTitle))
        {
            return Result<ScreeningDto>.Failure($"{eventTitle} is not valid.");
        }

        var eventSearched = await _eventService.GetByTitleAsync(eventTitle, ct);
        if (!eventSearched.IsSuccess)
        {
            return Result<ScreeningDto>
                .Failure(eventSearched.ErrorMessage ?? $"There is no event with {eventTitle}");
        }

        if (string.IsNullOrWhiteSpace(roomName))
        {
            return Result<ScreeningDto>.Failure($"{roomName} is not valid.");
        }
        
        var roomSearched = await _roomService.GetByNameAsync(roomName,ct);
        
        if (!roomSearched.IsSuccess)
        {
            return Result<ScreeningDto>
                .Failure(roomSearched.ErrorMessage ?? $"There is no room with name {roomName}");
        }

        if (dto.StartsAt <= DateTimeOffset.UtcNow.ToUniversalTime())
        {
            return Result<ScreeningDto>
                .Failure($"{dto.StartsAt} cannot be in past.");
        }

        var endsAt = dto.StartsAt.AddMinutes((eventSearched.Value?.DurationInMinutes ?? 0) + 10);
        
        var screening = new Screening
        {
            EventId = eventSearched.Value!.Id,
            RoomId = roomSearched.Value!.Id,
            StartsAt = dto.StartsAt,
            EndsAt = endsAt,
            Status = ScreeningStatus.Scheduled
        };

        var isBusy = await IsRoomBusy(screening, ct);
        
        if (isBusy)
        {
            return Result<ScreeningDto>.Failure($"Room is busy between {screening.StartsAt},{screening.EndsAt}");
        }

        try
        {
            _db.Screenings.Add(screening);
            await _db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex ) when 
            (ex.InnerException is PostgresException pg && pg.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            return Result<ScreeningDto>.Failure($"Room got booked already!");
        }

        return Result<ScreeningDto>
            .Success(
                new ScreeningDto(
                    screening.Id,
                    screening.EventId,
                    new EventSummaryDto(eventSearched.Value!.Id, eventSearched.Value.Title, eventSearched.Value.Genre, eventSearched.Value.DurationInMinutes),
                    screening.RoomId,
                    new RoomSummaryDto(roomSearched.Value!.Id, roomSearched.Value.Name, roomSearched.Value.SeatsNumber),
                    screening.StartsAt,
                    screening.EndsAt,
                    screening.Status.ToString()
                ));

    }

    public async Task<Result<ScreeningDto?>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var result = await _db.Screenings
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new ScreeningDto(
                s.Id,
                s.EventId,
                new EventSummaryDto(s.Event!.Id, s.Event.Title, s.Event.Genre, s.Event.DurationInMinutes),
                s.RoomId,
                new RoomSummaryDto(s.Room!.Id, s.Room.Name, s.Room.SeatsNumber),
                s.StartsAt,
                s.EndsAt,
                s.Status.ToString()
            ))
            .FirstOrDefaultAsync(ct);
        if (result is null)
        {
            return Result<ScreeningDto?>.Failure($"There is no screening with id {id}");
        }

        return Result<ScreeningDto?>.Success(result);
    }

    public async Task<Result<List<ScreeningListDto>>> GetAllAsync()
    {
        var result = await _db.Screenings
            .AsNoTracking()
            .Select(s =>
                new ScreeningListDto(
                    s.Id,
                    s.StartsAt,
                    s.EndsAt,
                    s.Status.ToString(),
                    s.RoomId
                ))
            .ToListAsync();
        return Result<List<ScreeningListDto>>.Success(result);
    }

    public async Task<bool> IsRoomBusy(Screening dto, CancellationToken ct)
    {
        var isRoomBusy = await _db.Screenings
            .AsNoTracking()
            .AnyAsync(s =>
                    s.RoomId == dto.RoomId &&
                    s.StartsAt < dto.EndsAt &&
                    s.EndsAt > dto.StartsAt && 
                    s.Status != ScreeningStatus.Cancelled,
                ct);
        return isRoomBusy;
    }
}