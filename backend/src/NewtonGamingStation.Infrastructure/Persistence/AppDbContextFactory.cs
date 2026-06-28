using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NewtonGamingStation.Infrastructure.Persistence;

/// <summary>
/// Design-time factory so the EF Core CLI (`dotnet ef migrations add ...`) can build
/// the context without the API host. The connection string only needs to be valid
/// syntactically at design time; it is not opened to scaffold a migration.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? "Server=localhost,1433;Database=NewtonGamingStation;User Id=sa;Password=Your_strong_Pass123;TrustServerCertificate=True;Encrypt=False";

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString, sql =>
                sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
            .Options;

        return new AppDbContext(options);
    }
}
