using CinemaProj.Data;
using CinemaProj.DTO;
using CinemaProj.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaProj.Services;

public class RoomService : IRoomService
{
    private readonly AppDbContext _db;
    private readonly ISeatService _seatService;
    public RoomService(AppDbContext db,ISeatService seatService)
    {
        _db = db;
        _seatService = seatService;
    }


    public async Task<Result<List<RoomListDto>>> GetAllAsync()
    {
        var result = await _db.Rooms.AsNoTracking().Select(r =>
            new RoomListDto(
                r.Id,
                r.Name,
                r.SeatsNumber,
                r.Screenings.Count
            )).ToListAsync();
        return Result<List<RoomListDto>>.Success(result);
    }

    public async Task<Result<RoomDto?>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var result = await _db.Rooms
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r =>
                new RoomDto(
                    r.Id,
                    r.Name,
                    r.SeatsNumber,
                    r.Seats
                        .OrderBy(s => s.Id)
                        .Select(s => new SeatDto(s.Id, s.SeatNumber))
                        .ToList(),
                    r.Screenings
                        .OrderBy(s => s.StartsAt)
                        .Select(s => new ScreeningDto(s.Id, s.StartsAt, s.EndsAt, s.Status.ToString(), s.RoomId))
                        .ToList()
                ))
            .FirstOrDefaultAsync(ct);
        if (result is null)
        {
            return Result<RoomDto?>.Failure($"There is no room with id {id}");
        }
        return Result<RoomDto?>.Success(result);
    }

    public async Task<Result<RoomDto>> CreateAsync(RegisterRoom dto, CancellationToken ct = default)
    {
        string name = (dto.Name ?? string.Empty).Trim();
        const int lowestNumberSeats = 10;
        const int highestNumberSeats = 100;
        const int maxLengthName = 100;
        if (string.IsNullOrWhiteSpace(name) || name.Length > maxLengthName)
        {
            return Result<RoomDto>.Failure($"{name} is not valid.");
        }

        if (dto.SeatNumber < lowestNumberSeats || dto.SeatNumber > highestNumberSeats)
        {
            return Result<RoomDto>.Failure(
                $"Number of seats must be in the range {lowestNumberSeats},{highestNumberSeats} ");
        }

        var newRoom = new Room
        {
            Name = name,
            SeatsNumber = dto.SeatNumber
        };
        newRoom.Seats = _seatService.GenerateLayout(newRoom.SeatsNumber, newRoom.Id);
        _db.Rooms.Add(newRoom);
        await _db.SaveChangesAsync(ct);
        return Result<RoomDto>.Success(
            new RoomDto(
                newRoom.Id,
                newRoom.Name,
                newRoom.SeatsNumber,
                newRoom.Seats
                    .Select(s=>new SeatDto(s.Id,s.SeatNumber.ToString()))
                    .ToList(),
                newRoom.Screenings
                    .Select(s =>
                        new ScreeningDto(
                            s.Id,
                            s.StartsAt,
                            s.EndsAt,
                            s.Status.ToString(),
                            s.RoomId))
                    .ToList())
            );
    }
}