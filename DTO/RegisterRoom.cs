using System.ComponentModel.DataAnnotations;

namespace CinemaProj.DTO;

public record RegisterRoom
{
    [Required]
    [MaxLength(100,ErrorMessage = "The name must be max 100 chars.")]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [Range(10,100,ErrorMessage = "The number of seats must be between 10 and 100")]
    public int SeatNumber { get; set; }
    
};