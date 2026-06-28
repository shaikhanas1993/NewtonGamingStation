using NewtonGamingStation.Application.Common;
using NewtonGamingStation.Application.Dtos;

namespace NewtonGamingStation.Application.Interfaces;

/// <summary>
/// Use-case surface for the catalogue. Controllers depend only on this, which keeps
/// HTTP concerns out of the business logic (SRP) and makes the logic unit-testable.
/// </summary>
public interface IGameService
{
    Task<PagedResult<GameDto>> SearchAsync(GameQueryParameters query, CancellationToken ct = default);

    Task<GameDto> GetByIdAsync(int id, CancellationToken ct = default);

    Task<GameDto> CreateAsync(CreateGameDto dto, CancellationToken ct = default);

    Task<GameDto> UpdateAsync(int id, UpdateGameDto dto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
