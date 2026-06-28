using NewtonGamingStation.Domain.Common;

namespace NewtonGamingStation.Domain.Entities;

/// <summary>
/// A single catalogue entry. The genre is modelled as an enum to keep filtering
/// simple and type-safe.
/// </summary>
public class Game : AuditableEntity
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public GameGenre Genre { get; set; }

    public string Platform { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public DateOnly ReleaseDate { get; set; }

    // Foreign key to the owning publisher.
    public int PublisherId { get; set; }

    public Publisher? Publisher { get; set; }
}
