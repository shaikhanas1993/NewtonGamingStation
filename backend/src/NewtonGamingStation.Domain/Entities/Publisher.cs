using NewtonGamingStation.Domain.Common;

namespace NewtonGamingStation.Domain.Entities;

/// <summary>
/// A company that publishes video games. One publisher can own many games.
/// </summary>
public class Publisher : AuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Country { get; set; }

    public ICollection<Game> Games { get; set; } = new List<Game>();
}
