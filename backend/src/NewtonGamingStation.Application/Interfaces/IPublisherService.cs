using NewtonGamingStation.Application.Dtos;

namespace NewtonGamingStation.Application.Interfaces;

public interface IPublisherService
{
    Task<IReadOnlyList<PublisherDto>> GetAllAsync(CancellationToken ct = default);
}
