using NewtonGamingStation.Domain.Entities;

namespace NewtonGamingStation.Application.Interfaces;

public interface IPublisherRepository
{
    Task<IReadOnlyList<Publisher>> GetAllAsync(CancellationToken ct = default);
}
