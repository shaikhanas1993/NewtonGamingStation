using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using NewtonGamingStation.Application.Common;
using NewtonGamingStation.Application.Dtos;
using NewtonGamingStation.Domain.Entities;
using Xunit;

namespace NewtonGamingStation.IntegrationTests;

/// <summary>
/// End-to-end tests through the HTTP pipeline against an in-memory SQLite database.
/// </summary>
public class GamesApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public GamesApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsSeededGames_Paged()
    {
        var page = await _client.GetFromJsonAsync<PagedResult<GameDto>>("/api/games?page=1&pageSize=2");

        page.Should().NotBeNull();
        page!.PageSize.Should().Be(2);
        page.Items.Should().HaveCount(2);
        page.TotalCount.Should().BeGreaterThanOrEqualTo(5);
    }

    [Fact]
    public async Task Get_WithSearch_FiltersByTitle()
    {
        var page = await _client.GetFromJsonAsync<PagedResult<GameDto>>("/api/games?search=witcher");

        page!.Items.Should().OnlyContain(g => g.Title.Contains("Witcher", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Get_WithGenreFilter_ReturnsOnlyThatGenre()
    {
        var page = await _client.GetFromJsonAsync<PagedResult<GameDto>>(
            $"/api/games?genre={(int)GameGenre.RolePlaying}");

        page!.Items.Should().NotBeEmpty();
        page.Items.Should().OnlyContain(g => g.Genre == GameGenre.RolePlaying);
    }

    [Fact]
    public async Task GetById_WhenMissing_Returns404()
    {
        var response = await _client.GetAsync("/api/games/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_CreatesGame_ThenGetById_ReturnsIt()
    {
        var create = new CreateGameDto
        {
            Title = "Integration Test Game",
            Description = "Created by a test.",
            Genre = GameGenre.Strategy,
            Platform = "PC",
            Price = 29.99m,
            ReleaseDate = new DateOnly(2023, 6, 1),
            PublisherId = 1
        };

        var postResponse = await _client.PostAsJsonAsync("/api/games", create);
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await postResponse.Content.ReadFromJsonAsync<GameDto>();
        created!.Id.Should().BeGreaterThan(0);

        var fetched = await _client.GetFromJsonAsync<GameDto>($"/api/games/{created.Id}");
        fetched!.Title.Should().Be("Integration Test Game");
    }

    [Fact]
    public async Task Post_WithInvalidPublisher_Returns404()
    {
        var create = new CreateGameDto
        {
            Title = "Orphan Game",
            Genre = GameGenre.Action,
            Platform = "PC",
            Price = 9.99m,
            ReleaseDate = new DateOnly(2023, 1, 1),
            PublisherId = 99999
        };

        var response = await _client.PostAsJsonAsync("/api/games", create);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_WithInvalidPayload_Returns400()
    {
        var create = new CreateGameDto
        {
            Title = "", // violates [Required]
            Platform = "PC",
            Genre = GameGenre.Action,
            PublisherId = 1
        };

        var response = await _client.PostAsJsonAsync("/api/games", create);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_UpdatesGame()
    {
        var create = new CreateGameDto
        {
            Title = "To Be Updated",
            Genre = GameGenre.Action,
            Platform = "PC",
            Price = 5m,
            ReleaseDate = new DateOnly(2022, 1, 1),
            PublisherId = 1
        };
        var created = await (await _client.PostAsJsonAsync("/api/games", create))
            .Content.ReadFromJsonAsync<GameDto>();

        var update = new UpdateGameDto
        {
            Title = "Updated Title",
            Genre = GameGenre.Puzzle,
            Platform = "Switch",
            Price = 12.5m,
            ReleaseDate = new DateOnly(2022, 2, 2),
            PublisherId = 1
        };

        var response = await _client.PutAsJsonAsync($"/api/games/{created!.Id}", update);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await response.Content.ReadFromJsonAsync<GameDto>();
        updated!.Title.Should().Be("Updated Title");
        updated.Genre.Should().Be(GameGenre.Puzzle);
    }

    [Fact]
    public async Task Delete_RemovesGame()
    {
        var create = new CreateGameDto
        {
            Title = "To Be Deleted",
            Genre = GameGenre.Action,
            Platform = "PC",
            Price = 1m,
            ReleaseDate = new DateOnly(2021, 1, 1),
            PublisherId = 1
        };
        var created = await (await _client.PostAsJsonAsync("/api/games", create))
            .Content.ReadFromJsonAsync<GameDto>();

        var deleteResponse = await _client.DeleteAsync($"/api/games/{created!.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/games/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPublishers_ReturnsSeeded()
    {
        var publishers = await _client.GetFromJsonAsync<List<PublisherDto>>("/api/publishers");
        publishers.Should().NotBeNull();
        publishers!.Should().HaveCountGreaterThanOrEqualTo(3);
    }
}
