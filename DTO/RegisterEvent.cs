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
    [Range(1,50,ErrorMessage = "Number of seats must be between 1 and 50.")]
    public int NumbersOfSeatings { get; set; } = 0;
}