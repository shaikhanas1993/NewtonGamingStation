using Microsoft.EntityFrameworkCore;
using NewtonGamingStation.Application.Common;
using NewtonGamingStation.Application.Interfaces;
using NewtonGamingStation.Domain.Entities;
using NewtonGamingStation.Infrastructure.Persistence;

namespace NewtonGamingStation.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IGameRepository"/>. All query composition
/// (search, filter, sort, paginate) lives here so the rest of the app stays
/// persistence-agnostic.
/// </summary>
public class GameRepository : IGameRepository
{
    private readonly AppDbContext _db;

    public GameRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<Game>> GetPagedAsync(GameQueryParameters query, CancellationToken ct = default)
    {
        IQueryable<Game> q = _db.Games
            .AsNoTracking()
            .Include(g => g.Publisher);

        // Search across title and description.
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim();
            q = q.Where(g =>
                EF.Functions.Like(g.Title, $"%{term}%") ||
                (g.Description != null && EF.Functions.Like(g.Description, $"%{term}%")));
        }

        // Filters.
        if (query.Genre.HasValue)
            q = q.Where(g => g.Genre == query.Genre.Value);

        if (!string.IsNullOrWhiteSpace(query.Platform))
            q = q.Where(g => g.Platform == query.Platform);

        if (query.PublisherId.HasValue)
            q = q.Where(g => g.PublisherId == query.PublisherId.Value);

        // Sorting.
        q = (query.SortBy?.ToLowerInvariant()) switch
        {
            "price" => query.Desc ? q.OrderByDescending(g => g.Price) : q.OrderBy(g => g.Price),
            "releasedate" => query.Desc ? q.OrderByDescending(g => g.ReleaseDate) : q.OrderBy(g => g.ReleaseDate),
            _ => query.Desc ? q.OrderByDescending(g => g.Title) : q.OrderBy(g => g.Title)
        };

        var totalCount = await q.CountAsync(ct);

        var items = await q
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(ct);

        return new PagedResult<Game>(items, totalCount, query.Page, query.PageSize);
    }

    public Task<Game?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.Games.Include(g => g.Publisher).FirstOrDefaultAsync(g => g.Id == id, ct);

    public async Task AddAsync(Game game, CancellationToken ct = default) =>
        await _db.Games.AddAsync(game, ct);

    public void Update(Game game) => _db.Games.Update(game);

    public void Remove(Game game) => _db.Games.Remove(game);

    public Task<bool> PublisherExistsAsync(int publisherId, CancellationToken ct = default) =>
        _db.Publishers.AnyAsync(p => p.Id == publisherId, ct);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _db.SaveChangesAsync(ct);
}
