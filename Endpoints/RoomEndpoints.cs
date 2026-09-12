using CinemaProj.DTO;
using CinemaProj.Services;

namespace CinemaProj.Endpoints;

public static class RoomEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/rooms");
        group.MapGet("/", GetRooms);
        group.MapGet("/{id:guid}", GetRoomById);
        group.MapPost("/", CreateRoom);
    }

    private static async Task<IResult> CreateRoom(RegisterRoom request, IRoomService roomService,CancellationToken ct = default)
    {
        var result = await roomService.CreateAsync(request, ct);
        if (!result.IsSuccess)
        {
            return Results.BadRequest(new { Error = result.ErrorMessage });
        }

        return Results.Created($"/api/rooms/{result.Value!.Id}", result.Value);
    }

    private static async Task<IResult> GetRoomById(Guid id, IRoomService roomService,CancellationToken ct = default)
    {
        var result = await roomService.GetByIdAsync(id, ct);
        if (!result.IsSuccess)
        {
            return Results.NotFound(new { Error = result.ErrorMessage });
        }

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> GetRooms(IRoomService roomService)
    {
        var result = await roomService.GetAllAsync();
        return Results.Ok(result.Value);
    }
}