namespace CinemaProj.DTO;

public record SeatDto(Guid Id, string SeatNumber);
public record EventListDto(Guid Id, string Title, string Genre, int DurationInMinutes, int ScreeningCount);
public record EventDto(Guid Id, string Title, string Genre, int DurationInMinutes, List<ScreeningListDto> Screenings);