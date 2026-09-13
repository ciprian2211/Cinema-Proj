using CinemaProj.DTO;
using CinemaProj.Models;

namespace CinemaProj.Services;

public interface IScreeningService
{
    Task<Result<ScreeningDto>> CreateAsync(RegisterScreening dto, CancellationToken ct = default);
    Task<Result<ScreeningDto?>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<List<ScreeningListDto>>> GetAllAsync();
    //TO DO: search screening by event
    Task<bool> IsRoomBusy(Screening dto, CancellationToken ct = default);
}