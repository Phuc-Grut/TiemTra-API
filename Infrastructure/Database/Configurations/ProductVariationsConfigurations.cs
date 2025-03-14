using Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Configurations
{
    public class ProductVariationsConfigurations : IEntityTypeConfiguration<ProductVariations>
    {
        public void Configure(EntityTypeBuilder<ProductVariations> builder)
        {
            builder.ToTable("ProductVariations");

            builder.HasKey(pv => pv.ProductVariationsId);

            builder.HasOne(pv => pv.Product)
              .WithMany(p => p.ProductVariations)
              .HasForeignKey(pv => pv.ProductId);

            builder.HasMany(pv => pv.ProductImages)
               .WithOne(pi => pi.ProductVariation)
               .HasForeignKey(pi => pi.ProductVariationId)
               .OnDelete(DeleteBehavior.NoAction);

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
