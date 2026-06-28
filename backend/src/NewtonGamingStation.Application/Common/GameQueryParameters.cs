using NewtonGamingStation.Domain.Entities;

namespace NewtonGamingStation.Application.Common;

/// <summary>
/// Carries the search, filter, sort and pagination options for the catalogue.
/// Page size is clamped so a client cannot ask for an unbounded result set.
/// </summary>
public class GameQueryParameters
{
    private const int MaxPageSize = 100;
    private int _pageSize = 12;
    private int _page = 1;

    /// <summary>Free-text search across title and description.</summary>
    public string? Search { get; set; }

    /// <summary>Optional genre filter.</summary>
    public GameGenre? Genre { get; set; }

    /// <summary>Optional platform filter (exact, case-insensitive).</summary>
    public string? Platform { get; set; }

    /// <summary>Optional publisher filter by id.</summary>
    public int? PublisherId { get; set; }

    /// <summary>Field to sort by: title, price, releaseDate. Defaults to title.</summary>
    public string SortBy { get; set; } = "title";

    /// <summary>Sort direction: asc or desc.</summary>
    public bool Desc { get; set; }

    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 1 : (value > MaxPageSize ? MaxPageSize : value);
    }
}
