using FluentAssertions;
using Moq;
using NewtonGamingStation.Application.Common;
using NewtonGamingStation.Application.Dtos;
using NewtonGamingStation.Application.Interfaces;
using NewtonGamingStation.Application.Services;
using NewtonGamingStation.Domain.Entities;
using Xunit;

namespace NewtonGamingStation.UnitTests;

/// <summary>
/// Pure unit tests for <see cref="GameService"/>. The repository is mocked, so these
/// exercise business rules only — no database involved.
/// </summary>
public class GameServiceTests
{
    private readonly Mock<IGameRepository> _repo = new();
    private readonly GameService _sut;

    public GameServiceTests()
    {
        _sut = new GameService(_repo.Object);
    }

    private static Game SampleGame(int id = 1) => new()
    {
        Id = id,
        Title = "Test Game",
        Genre = GameGenre.Action,
        Platform = "PC",
        Price = 19.99m,
        ReleaseDate = new DateOnly(2024, 1, 1),
        PublisherId = 1,
        Publisher = new Publisher { Id = 1, Name = "Test Publisher" }
    };

    [Fact]
    public async Task SearchAsync_MapsEntitiesToDtos()
    {
        var paged = new PagedResult<Game>(new[] { SampleGame() }, totalCount: 1, page: 1, pageSize: 12);
        _repo.Setup(r => r.GetPagedAsync(It.IsAny<GameQueryParameters>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(paged);

        var result = await _sut.SearchAsync(new GameQueryParameters());

        result.TotalCount.Should().Be(1);
        result.Items.Should().ContainSingle()
            .Which.PublisherName.Should().Be("Test Publisher");
    }

    [Fact]
    public async Task GetByIdAsync_WhenMissing_ThrowsNotFound()
    {
        _repo.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
             .ReturnsAsync((Game?)null);

        var act = () => _sut.GetByIdAsync(99);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_WhenPublisherMissing_ThrowsNotFound()
    {
        _repo.Setup(r => r.PublisherExistsAsync(5, It.IsAny<CancellationToken>()))
             .ReturnsAsync(false);

        var dto = new CreateGameDto { Title = "X", Platform = "PC", Genre = GameGenre.Action, PublisherId = 5 };
        var act = () => _sut.CreateAsync(dto);

        await act.Should().ThrowAsync<NotFoundException>();
        _repo.Verify(r => r.AddAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenValid_PersistsAndReturnsDto()
    {
        _repo.Setup(r => r.PublisherExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _repo.Setup(r => r.AddAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()))
             .Returns(Task.CompletedTask);
        _repo.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(SampleGame());

        var dto = new CreateGameDto
        {
            Title = "  Trimmed Title  ",
            Platform = "PC",
            Genre = GameGenre.Action,
            Price = 10m,
            ReleaseDate = new DateOnly(2024, 1, 1),
            PublisherId = 1
        };

        var result = await _sut.CreateAsync(dto);

        result.Should().NotBeNull();
        _repo.Verify(r => r.AddAsync(It.Is<Game>(g => g.Title == "Trimmed Title"), It.IsAny<CancellationToken>()), Times.Once);
        _repo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenExists_RemovesAndSaves()
    {
        var game = SampleGame();
        _repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(game);
        _repo.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _sut.DeleteAsync(1);

        _repo.Verify(r => r.Remove(game), Times.Once);
        _repo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenMissing_ThrowsNotFound()
    {
        _repo.Setup(r => r.GetByIdAsync(7, It.IsAny<CancellationToken>())).ReturnsAsync((Game?)null);

        var dto = new UpdateGameDto { Title = "X", Platform = "PC", Genre = GameGenre.Action, PublisherId = 1 };
        var act = () => _sut.UpdateAsync(7, dto);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
