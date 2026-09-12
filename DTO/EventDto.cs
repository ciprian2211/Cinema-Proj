namespace CinemaProj.DTO;

public record ScreeningDto(Guid Id, DateTimeOffset StartsAt, DateTimeOffset EndsAt, string Status, Guid RoomId);

public record SeatDto(Guid Id, string SeatNumber);
public record EventListDto(Guid Id, string Title, string Genre, int ScreeningCount);

public record EventDto(Guid Id, string Title, string Genre, List<ScreeningDto> Screenings);