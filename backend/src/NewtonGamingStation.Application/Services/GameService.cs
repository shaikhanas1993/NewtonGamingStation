using NewtonGamingStation.Application.Common;
using NewtonGamingStation.Application.Dtos;
using NewtonGamingStation.Application.Interfaces;
using NewtonGamingStation.Application.Mapping;

namespace NewtonGamingStation.Application.Services;

/// <summary>
/// Implements the catalogue use cases. It validates business rules and orchestrates
/// the repository; it knows nothing about HTTP or EF Core.
/// </summary>
public class GameService : IGameService
{
    private readonly IGameRepository _repository;

    public GameService(IGameRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<GameDto>> SearchAsync(GameQueryParameters query, CancellationToken ct = default)
    {
        var page = await _repository.GetPagedAsync(query, ct);
        var dtos = page.Items.Select(g => g.ToDto()).ToList();
        return new PagedResult<GameDto>(dtos, page.TotalCount, page.Page, page.PageSize);
    }

    public async Task<GameDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var game = await _repository.GetByIdAsync(id, ct)
                   ?? throw new NotFoundException(nameof(Domain.Entities.Game), id);
        return game.ToDto();
    }

    public async Task<GameDto> CreateAsync(CreateGameDto dto, CancellationToken ct = default)
    {
        await EnsurePublisherExists(dto.PublisherId, ct);

        var game = dto.ToEntity();
        await _repository.AddAsync(game, ct);
        await _repository.SaveChangesAsync(ct);

        // Re-read so the publisher navigation is populated for the response.
        var created = await _repository.GetByIdAsync(game.Id, ct);
        return created!.ToDto();
    }

    public async Task<GameDto> UpdateAsync(int id, UpdateGameDto dto, CancellationToken ct = default)
    {
        var game = await _repository.GetByIdAsync(id, ct)
                   ?? throw new NotFoundException(nameof(Domain.Entities.Game), id);

        await EnsurePublisherExists(dto.PublisherId, ct);

        dto.Apply(game);
        _repository.Update(game);
        await _repository.SaveChangesAsync(ct);

        var updated = await _repository.GetByIdAsync(id, ct);
        return updated!.ToDto();
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var game = await _repository.GetByIdAsync(id, ct)
                   ?? throw new NotFoundException(nameof(Domain.Entities.Game), id);

        _repository.Remove(game);
        await _repository.SaveChangesAsync(ct);
    }

    private async Task EnsurePublisherExists(int publisherId, CancellationToken ct)
    {
        if (!await _repository.PublisherExistsAsync(publisherId, ct))
            throw new NotFoundException(nameof(Domain.Entities.Publisher), publisherId);
    }
}
