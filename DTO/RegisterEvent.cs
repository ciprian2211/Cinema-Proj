using System.ComponentModel.DataAnnotations;
using CinemaProj.Models;

namespace CinemaProj.DTO;

public record RegisterEvent
{
    [Required]
    [MaxLength(150,ErrorMessage = "Title should not be more than 150 characters long.")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(100,ErrorMessage = "Genre should not be more than 100 characters long.")]
    public string Genre { get; set; } = string.Empty;
    [Required]
    [Range(10,100,ErrorMessage = "Number of seats must be between 10 and 100 .")]
    public int SeatsNumber { get; set; } = 0;

    [Range(30,300)]
    public int DurationInMinutes { get; set; } = 120;
    public List<RegisterScreening> Screenings { get; set; } = new();
}