using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MerchStore.Domain.Entities;

namespace MerchStore.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuration class for the Product entity.
/// This defines how a Product is mapped to the database.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.StockQuantity)
            .IsRequired();

        builder.Property(p => p.ImageUrl)
            .IsRequired(false);

        // ✅ Updated: Price is now a simple decimal
        builder.Property(p => p.Price)
            .HasColumnName("Price")
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasIndex(p => p.Name);
    }
}
