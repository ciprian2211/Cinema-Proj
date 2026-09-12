using System;
using System.Threading;
using System.Threading.Tasks;
using CinemaProj.DTO;
using CinemaProj.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CinemaProj.Endpoints;

public static class EventEndpoints 
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/events");
        group.MapGet("/", GetEvents);
        group.MapGet("/{id:guid}", GetEventById);
        group.MapPost("/", CreateEvent);
    }

    private static async Task<IResult> CreateEvent(RegisterEvent request, IEventService eventService, CancellationToken ct)
    {
        var result = await eventService.CreateAsync(request, ct);
        
        if (!result.IsSuccess)
        {
            return Results.BadRequest(new { Error = result.ErrorMessage });
        }

        return Results.Created($"/api/events/{result.Value!.Id}", result.Value);
    }

    private static async Task<IResult> GetEvents(IEventService eventService)
    {
        var result = await eventService.GetAllAsync();
        return Results.Ok(result.Value);
    }

    private static async Task<IResult> GetEventById(IEventService eventService, Guid id)
    {
        var result = await eventService.GetByIdAsync(id);
        
        if (!result.IsSuccess)
        {
            return Results.NotFound(new { Error = result.ErrorMessage });
        }

        return Results.Ok(result.Value);
    }
}