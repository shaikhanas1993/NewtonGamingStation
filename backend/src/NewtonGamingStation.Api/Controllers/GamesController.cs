using Microsoft.AspNetCore.Mvc;
using NewtonGamingStation.Application.Common;
using NewtonGamingStation.Application.Dtos;
using NewtonGamingStation.Application.Interfaces;

namespace NewtonGamingStation.Api.Controllers;

/// <summary>
/// REST endpoints for the games catalogue. The controller is intentionally thin:
/// it only translates HTTP to service calls and back (SRP).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GamesController : ControllerBase
{
    private readonly IGameService _service;

    public GamesController(IGameService service)
    {
        _service = service;
    }

    /// <summary>Search, filter, sort and paginate the catalogue.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<GameDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<GameDto>>> Search(
        [FromQuery] GameQueryParameters query, CancellationToken ct)
    {
        var result = await _service.SearchAsync(query, ct);
        return Ok(result);
    }

    /// <summary>Get a single game by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GameDto>> GetById(int id, CancellationToken ct)
    {
        var game = await _service.GetByIdAsync(id, ct);
        return Ok(game);
    }

    /// <summary>Create a new game.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GameDto>> Create([FromBody] CreateGameDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Update an existing game.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GameDto>> Update(int id, [FromBody] UpdateGameDto dto, CancellationToken ct)
    {
        var updated = await _service.UpdateAsync(id, dto, ct);
        return Ok(updated);
    }

    /// <summary>Delete a game.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        return NoContent();
    }
}
