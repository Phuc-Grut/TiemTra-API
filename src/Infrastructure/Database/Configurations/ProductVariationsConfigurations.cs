using Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class ProductVariationsConfigurations : IEntityTypeConfiguration<ProductVariations>
    {
        public void Configure(EntityTypeBuilder<ProductVariations> builder)
        {
            builder.ToTable("ProductVariations");

            builder.HasKey(pv => pv.ProductVariationId);

            builder.HasOne(pv => pv.Product)
              .WithMany(p => p.ProductVariations)
              .HasForeignKey(pv => pv.ProductId);

            builder.Property(pv => pv.CreatedBy).IsRequired();
            builder.Property(pv => pv.UpdatedBy).IsRequired();

            builder.Property(pv => pv.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(pv => pv.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}