namespace CinemaProj.DTO;

public record RoomListDto(Guid Id,string Name,int SeatsNumber,int ScreeningCount);
public record RoomDto(Guid Id,string Name, int SeatsNumber,List<SeatDto> Seats,List<ScreeningListDto> Screenings);
public record RoomSummaryDto(Guid Id, string Name, int SeatsNumber);