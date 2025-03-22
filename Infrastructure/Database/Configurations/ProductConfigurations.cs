using Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p => p.ProductId);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(p => p.ProductImages)
              .WithOne(pi => pi.Product)   // Mỗi ProductImage thuộc về một Product
              .HasForeignKey(pi => pi.ProductId)
              .OnDelete(DeleteBehavior.NoAction);

            // Product có thể có nhiều ProductVariations
            builder.HasMany(p => p.ProductVariations)
                   .WithOne(pv => pv.Product)  // Mỗi ProductVariation thuộc về một Product
                   .HasForeignKey(pv => pv.ProductId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.Property(p => p.Price)
              .HasColumnType("decimal(18, 2)");

            builder.Property(p => p.CreatedBy).IsRequired();
            builder.Property(p => p.UpdatedBy).IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}