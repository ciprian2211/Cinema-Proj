using System.ComponentModel.DataAnnotations;

namespace CinemaProj.DTO;

public record RegisterScreening
{
    [Required]
    public DateTimeOffset StartsAt { get; set; }
}