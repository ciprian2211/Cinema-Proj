using CinemaProj.DTO;
using CinemaProj.Models;

namespace CinemaProj.Services;

public interface IEventService
{
     Task<List<EventListDto>> GetAllAsync();
     Task<EventDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
     Task<EventDto> CreateAsync(RegisterEvent dto,CancellationToken ct = default);
}