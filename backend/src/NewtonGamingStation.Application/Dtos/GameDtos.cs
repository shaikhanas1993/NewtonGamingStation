using System.ComponentModel.DataAnnotations;
using NewtonGamingStation.Domain.Entities;

namespace NewtonGamingStation.Application.Dtos;

/// <summary>Read model returned to clients for a catalogue entry.</summary>
public record GameDto(
    int Id,
    string Title,
    string? Description,
    GameGenre Genre,
    string GenreName,
    string Platform,
    decimal Price,
    DateOnly ReleaseDate,
    int PublisherId,
    string PublisherName);

/// <summary>Payload for creating a game.</summary>
public class CreateGameDto
{
    [Required, StringLength(200, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    [Required]
    public GameGenre Genre { get; set; }

    [Required, StringLength(100)]
    public string Platform { get; set; } = string.Empty;

    [Range(0, 100000)]
    public decimal Price { get; set; }

    [Required]
    public DateOnly ReleaseDate { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A valid PublisherId is required.")]
    public int PublisherId { get; set; }
}

/// <summary>Payload for updating a game. Same shape as create for this simple domain.</summary>
public class UpdateGameDto : CreateGameDto
{
}
