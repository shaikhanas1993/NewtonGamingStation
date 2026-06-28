using NewtonGamingStation.Domain.Common;

namespace NewtonGamingStation.Domain.Entities;

/// <summary>
/// An application role (e.g. Administrator, Editor, Viewer). Many users can share
/// a role and a user can hold many roles via the <see cref="UserRole"/> join.
/// </summary>
public class Role : AuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
