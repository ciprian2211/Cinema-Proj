using System.Text.Json.Serialization;
using CinemaProj.Data;
using CinemaProj.Endpoints;
using CinemaProj.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IRoomService, RoomService>();

builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddProblemDetails();

var app = builder.Build();
//endpoints
EventEndpoints.Map(app);
RoomEndpoints.Map(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();
//app.UseHttpsRedirection();
app.Run();

