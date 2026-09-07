using System.Data.Common;
using CinemaProj.Data;
using CinemaProj.DTO;
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
        group.MapGet("/", CreateEvent);

    }

    private static async Task<IResult> CreateEvent(RegisterEvent request, IEventService eventService)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return Results.BadRequest("Title is not valid.");
        }

        if (request.NumbersOfSeatings <= 0)
        {
            return Results.BadRequest("Number of seatings must be greater than 0 and less or equal to 50.");
        }

        var registeredEvent = await eventService.CreateAsync(request);
        return Results.Created($"/api/events/{registeredEvent.Id}", registeredEvent);
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