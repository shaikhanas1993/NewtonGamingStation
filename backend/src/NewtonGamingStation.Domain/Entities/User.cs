using NewtonGamingStation.Domain.Common;

namespace NewtonGamingStation.Domain.Entities;

/// <summary>
/// An application user. Auth is intentionally out of scope (per the brief), so this
/// only carries identity metadata and the role assignments.
/// </summary>
public class User : AuditableEntity
{
    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
