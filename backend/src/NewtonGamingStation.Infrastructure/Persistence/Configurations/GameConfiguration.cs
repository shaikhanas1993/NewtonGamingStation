using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewtonGamingStation.Domain.Entities;

namespace NewtonGamingStation.Infrastructure.Persistence.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Games");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(g => g.Description)
            .HasMaxLength(2000);

        builder.Property(g => g.Platform)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.Genre)
            .HasConversion<int>();

        builder.Property(g => g.Price)
            .HasColumnType("decimal(10,2)");

        builder.HasIndex(g => g.Title);

        builder.HasOne(g => g.Publisher)
            .WithMany(p => p.Games)
            .HasForeignKey(g => g.PublisherId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
