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
    public class ProductVariationDetailsConfigurations : IEntityTypeConfiguration<ProductVariationDetails>
    {
        public void Configure(EntityTypeBuilder<ProductVariationDetails> builder)
        {
            builder.ToTable("ProductVariationDetails");
            builder.HasKey(pvd => pvd.ProductVariationDetailsId);
            builder.HasOne(pvd => pvd.ProductVariation)
               .WithMany(pv => pv.ProductVariationDetails)
               .HasForeignKey(pvd => pvd.ProductVariationsId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Property(pvd => pvd.Price)
              .HasColumnType("decimal(18, 2)");

            builder.Property(pvd => pvd.CreatedBy).IsRequired();
            builder.Property(pvd => pvd.UpdatedBy).IsRequired();

            builder.Property(pvd => pvd.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(pvd => pvd.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
