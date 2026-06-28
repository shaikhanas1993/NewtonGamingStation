using NewtonGamingStation.Application.Common;
using NewtonGamingStation.Domain.Entities;

namespace NewtonGamingStation.Application.Interfaces;

/// <summary>
/// Persistence abstraction for games. The application layer depends on this
/// interface, not on EF Core, satisfying the Dependency Inversion Principle.
/// </summary>
public interface IGameRepository
{
    Task<PagedResult<Game>> GetPagedAsync(GameQueryParameters query, CancellationToken ct = default);

    Task<Game?> GetByIdAsync(int id, CancellationToken ct = default);

    Task AddAsync(Game game, CancellationToken ct = default);

    void Update(Game game);

    void Remove(Game game);

    Task<bool> PublisherExistsAsync(int publisherId, CancellationToken ct = default);

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
