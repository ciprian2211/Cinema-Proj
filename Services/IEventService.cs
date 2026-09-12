using CinemaProj.DTO;
using CinemaProj.DTO;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaProj.Services;

public interface IEventService
{
    Task<Result<List<EventListDto>>> GetAllAsync();
    Task<Result<EventDto?>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<EventDto>> CreateAsync(RegisterEvent dto, CancellationToken ct = default);
}