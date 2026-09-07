using CinemaProj.DTO;
using CinemaProj.Models;

namespace CinemaProj.Services;

public interface IEventService
{
     Task<List<Event>> GetAllAsync();
     Task<Event> GetByIdAsync(Guid id);
     Task<Event> CreateAsync(RegisterEvent dto);
}