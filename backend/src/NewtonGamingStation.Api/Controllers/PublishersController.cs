using Microsoft.AspNetCore.Mvc;
using NewtonGamingStation.Application.Dtos;
using NewtonGamingStation.Application.Interfaces;

namespace NewtonGamingStation.Api.Controllers;

/// <summary>
/// Read-only publisher endpoint used to populate the publisher dropdown on the
/// add/edit form.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PublishersController : ControllerBase
{
    private readonly IPublisherService _service;

    public PublishersController(IPublisherService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PublisherDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PublisherDto>>> GetAll(CancellationToken ct)
    {
        var publishers = await _service.GetAllAsync(ct);
        return Ok(publishers);
    }
}
