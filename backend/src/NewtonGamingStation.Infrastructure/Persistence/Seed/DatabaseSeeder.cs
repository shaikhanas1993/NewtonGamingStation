using Microsoft.EntityFrameworkCore;
using NewtonGamingStation.Domain.Entities;

namespace NewtonGamingStation.Infrastructure.Persistence.Seed;

/// <summary>
/// Applies pending migrations and seeds reference + demo data on startup. Idempotent:
/// it only inserts when a table is empty, so it is safe to run on every boot.
/// </summary>
public static class DatabaseSeeder
{
    public static async Task MigrateAndSeedAsync(AppDbContext db, CancellationToken ct = default)
    {
        await db.Database.MigrateAsync(ct);
        await SeedAsync(db, ct);
    }

    /// <summary>
    /// Seeds reference + demo data without touching migrations. Used by tests that
    /// create the schema with EnsureCreated on a different provider (SQLite).
    /// </summary>
    public static async Task SeedAsync(AppDbContext db, CancellationToken ct = default)
    {
        await SeedRolesAndUsersAsync(db, ct);
        await SeedPublishersAndGamesAsync(db, ct);

        await db.SaveChangesAsync(ct);
    }

    private static async Task SeedRolesAndUsersAsync(AppDbContext db, CancellationToken ct)
    {
        if (await db.Roles.AnyAsync(ct)) return;

        var admin = new Role { Name = "Administrator", Description = "Full access to manage the catalogue." };
        var editor = new Role { Name = "Editor", Description = "Can create and edit games." };
        var viewer = new Role { Name = "Viewer", Description = "Read-only access." };
        db.Roles.AddRange(admin, editor, viewer);

        var alice = new User { UserName = "alice", Email = "alice@newton.local", DisplayName = "Alice Admin" };
        var bob = new User { UserName = "bob", Email = "bob@newton.local", DisplayName = "Bob Editor" };
        db.Users.AddRange(alice, bob);

        // Save first so identity keys are generated for the join rows.
        await db.SaveChangesAsync(ct);

        db.UserRoles.AddRange(
            new UserRole { UserId = alice.Id, RoleId = admin.Id, AssignedAtUtc = DateTimeOffset.UtcNow },
            new UserRole { UserId = bob.Id, RoleId = editor.Id, AssignedAtUtc = DateTimeOffset.UtcNow });
    }

    private static async Task SeedPublishersAndGamesAsync(AppDbContext db, CancellationToken ct)
    {
        if (await db.Publishers.AnyAsync(ct)) return;

        var nintendo = new Publisher { Name = "Nintendo", Country = "Japan" };
        var cdpr = new Publisher { Name = "CD Projekt Red", Country = "Poland" };
        var valve = new Publisher { Name = "Valve", Country = "USA" };
        db.Publishers.AddRange(nintendo, cdpr, valve);
        await db.SaveChangesAsync(ct);

        db.Games.AddRange(
            new Game
            {
                Title = "The Legend of Zelda: Breath of the Wild",
                Description = "Open-world action-adventure set in Hyrule.",
                Genre = GameGenre.Adventure,
                Platform = "Nintendo Switch",
                Price = 59.99m,
                ReleaseDate = new DateOnly(2017, 3, 3),
                PublisherId = nintendo.Id
            },
            new Game
            {
                Title = "The Witcher 3: Wild Hunt",
                Description = "Story-driven open-world RPG.",
                Genre = GameGenre.RolePlaying,
                Platform = "PC",
                Price = 39.99m,
                ReleaseDate = new DateOnly(2015, 5, 19),
                PublisherId = cdpr.Id
            },
            new Game
            {
                Title = "Cyberpunk 2077",
                Description = "Open-world RPG in Night City.",
                Genre = GameGenre.RolePlaying,
                Platform = "PC",
                Price = 49.99m,
                ReleaseDate = new DateOnly(2020, 12, 10),
                PublisherId = cdpr.Id
            },
            new Game
            {
                Title = "Half-Life: Alyx",
                Description = "VR-first first-person shooter.",
                Genre = GameGenre.Shooter,
                Platform = "PC",
                Price = 49.99m,
                ReleaseDate = new DateOnly(2020, 3, 23),
                PublisherId = valve.Id
            },
            new Game
            {
                Title = "Mario Kart 8 Deluxe",
                Description = "Kart racing party game.",
                Genre = GameGenre.Racing,
                Platform = "Nintendo Switch",
                Price = 59.99m,
                ReleaseDate = new DateOnly(2017, 4, 28),
                PublisherId = nintendo.Id
            });
    }
}
