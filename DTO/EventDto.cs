namespace CinemaProj.DTO;

public record ScreeningDto(Guid Id, DateTimeOffset StartsAt, DateTimeOffset EndsAt,string Status);

public record EventListDto(Guid Id, string Title, string Genre,
    int SeatsNumber, int ScreeningCount);
public record EventDto(Guid Id, string Title,string Genre, 
    int SeatsNumber,List<string> Seats,List<ScreeningDto> Screenings);
    