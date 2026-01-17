using AirlineService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirlineService.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the Flight entity.
/// </summary>
public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Origin)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(f => f.Destination)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(f => f.Departure)
            .IsRequired();

        builder.Property(f => f.Arrival)
            .IsRequired();

        builder.Property(f => f.Status)
            .IsRequired();
    }
}

