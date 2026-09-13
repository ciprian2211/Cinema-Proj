using System.ComponentModel.DataAnnotations;
using CinemaProj.Models;

namespace CinemaProj.DTO;

public record RegisterScreening
{
    [Required] 
    public string EventTitle { get; set; } = string.Empty;

    [Required] 
    public string RoomName { get; set; } = string.Empty;
    
    [Required]
    public DateTimeOffset StartsAt { get; set; }
    
    
}