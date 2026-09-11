using System.Data.Common;
using CinemaProj.Data;
using CinemaProj.DTO;
using CinemaProj.Exceptions;
using CinemaProj.Services;
using Microsoft.EntityFrameworkCore;

namespace CinemaProj.Endpoints;

public static class EventEndpoints 
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/events");
        group.MapGet("/",GetEvents);
        group.MapGet("/{id:guid}", GetEventById);
        group.MapPost("/", CreateEvent);

    }

    private static async Task<IResult> CreateEvent(RegisterEvent request, IEventService eventService,CancellationToken ct)
    {
        try
        {
            var e = await eventService.CreateAsync(request, ct);
            return Results.Created($"/api/events/{e.Id}", e);
        }
        catch (ValidationException validationException)
        {
            return Results.BadRequest(new { validationException.Code, validationException.Message });
        }
        catch (DuplicateScreeningException duplicateScreeningException)
        {
            return Results.Conflict(new { duplicateScreeningException.Message });
        }
        
    }

    private static async Task<IResult> GetEvents(IEventService eventService)
    {
        var events = await eventService.GetAllAsync();

        return Results.Ok(events);
    }

    private static async Task<IResult> GetEventById(IEventService eventService,Guid id)
    {
        var searchedEvent = await eventService.GetByIdAsync(id);
        if (searchedEvent is null)
        {
            return Results.NotFound($"The event with {id} does not exist.");
        }

        return Results.Ok(searchedEvent);
    }
}