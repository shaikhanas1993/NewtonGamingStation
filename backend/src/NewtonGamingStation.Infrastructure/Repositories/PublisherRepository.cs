using Microsoft.EntityFrameworkCore;
using NewtonGamingStation.Application.Interfaces;
using NewtonGamingStation.Domain.Entities;
using NewtonGamingStation.Infrastructure.Persistence;

namespace NewtonGamingStation.Infrastructure.Repositories;

public class PublisherRepository : IPublisherRepository
{
    private readonly AppDbContext _db;

    public PublisherRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Publisher>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Publishers.AsNoTracking().OrderBy(p => p.Name).ToListAsync(ct);
}
