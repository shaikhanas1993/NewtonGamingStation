namespace NewtonGamingStation.Domain.Entities;

/// <summary>
/// Explicit join entity for the many-to-many relationship between users and roles.
/// Modelling it explicitly keeps room for future columns (e.g. AssignedAtUtc).
/// </summary>
public class UserRole
{
    public int UserId { get; set; }
    public User? User { get; set; }

    public int RoleId { get; set; }
    public Role? Role { get; set; }

    public DateTimeOffset AssignedAtUtc { get; set; }
}
