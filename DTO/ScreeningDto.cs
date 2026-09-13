using CinemaProj.Models;

namespace CinemaProj.DTO;

public record ScreeningListDto(Guid Id, DateTimeOffset StartsAt, DateTimeOffset EndsAt, string Status, Guid RoomId);
public record ScreeningDto(
    Guid Id,
    Guid EventId,
    EventSummaryDto Event,
    Guid RoomId,
    RoomSummaryDto Room,
    DateTimeOffset StartsAt,
    DateTimeOffset EndsAt,
    string Status
);