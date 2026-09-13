using CinemaProj.DTO;
using CinemaProj.Models;

namespace CinemaProj.Services;

public interface IRoomService
{
    Task<Result<List<RoomListDto>>> GetAllAsync();
    Task<Result<RoomDto?>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<RoomDto>> CreateAsync(RegisterRoom dto, CancellationToken ct = default);
    Task<Result<RoomDto?>> GetByNameAsync(string name, CancellationToken ct = default);
}