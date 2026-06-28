using FluentAssertions;
using NewtonGamingStation.Application.Common;
using Xunit;

namespace NewtonGamingStation.UnitTests;

public class GameQueryParametersTests
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(-5, 1)]
    [InlineData(3, 3)]
    public void Page_IsClampedToAtLeastOne(int input, int expected)
    {
        var p = new GameQueryParameters { Page = input };
        p.Page.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(50, 50)]
    [InlineData(500, 100)]
    public void PageSize_IsClampedToRange(int input, int expected)
    {
        var p = new GameQueryParameters { PageSize = input };
        p.PageSize.Should().Be(expected);
    }
}
