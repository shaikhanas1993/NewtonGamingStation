namespace NewtonGamingStation.Domain.Common;

/// <summary>
/// Base type that every persisted entity derives from. Centralising the key and
/// audit columns keeps the entities focused on their own data (SRP) and lets the
/// DbContext stamp audit values in one place.
/// </summary>
public abstract class AuditableEntity
{
    public int Id { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }

    public DateTimeOffset? UpdatedAtUtc { get; set; }
}
