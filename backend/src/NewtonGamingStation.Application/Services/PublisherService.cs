using NewtonGamingStation.Application.Dtos;
using NewtonGamingStation.Application.Interfaces;
using NewtonGamingStation.Application.Mapping;

namespace NewtonGamingStation.Application.Services;

public class PublisherService : IPublisherService
{
    private readonly IPublisherRepository _repository;

    public PublisherService(IPublisherRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<PublisherDto>> GetAllAsync(CancellationToken ct = default)
    {
        var publishers = await _repository.GetAllAsync(ct);
        return publishers.Select(p => p.ToDto()).ToList();
    }
}
