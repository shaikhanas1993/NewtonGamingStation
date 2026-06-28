using NewtonGamingStation.Application.Dtos;
using NewtonGamingStation.Domain.Entities;

namespace NewtonGamingStation.Application.Mapping;

/// <summary>
/// Hand-rolled mapping extensions. Avoiding a mapping framework keeps the demo
/// transparent and dependency-light while still isolating mapping in one place.
/// </summary>
public static class GameMappings
{
    public static GameDto ToDto(this Game game) => new(
        game.Id,
        game.Title,
        game.Description,
        game.Genre,
        game.Genre.ToString(),
        game.Platform,
        game.Price,
        game.ReleaseDate,
        game.PublisherId,
        game.Publisher?.Name ?? string.Empty);

    public static Game ToEntity(this CreateGameDto dto) => new()
    {
        Title = dto.Title.Trim(),
        Description = dto.Description?.Trim(),
        Genre = dto.Genre,
        Platform = dto.Platform.Trim(),
        Price = dto.Price,
        ReleaseDate = dto.ReleaseDate,
        PublisherId = dto.PublisherId
    };

    public static void Apply(this UpdateGameDto dto, Game game)
    {
        game.Title = dto.Title.Trim();
        game.Description = dto.Description?.Trim();
        game.Genre = dto.Genre;
        game.Platform = dto.Platform.Trim();
        game.Price = dto.Price;
        game.ReleaseDate = dto.ReleaseDate;
        game.PublisherId = dto.PublisherId;
    }

    public static PublisherDto ToDto(this Publisher publisher) =>
        new(publisher.Id, publisher.Name, publisher.Country);
}
