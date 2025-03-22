using Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class ProductAttributeConfigurations : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            builder.ToTable("ProductAttributes");
            builder.HasKey(pa => pa.ProductAttributeId);

            builder.Property(pa => pa.ProductId)
               .IsRequired(false);

            builder.HasOne(pa => pa.Product)
                .WithMany(p => p.ProductAttributes)
                .HasForeignKey(pa => pa.ProductId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(pa => pa.Attribute)
                .WithMany(a => a.ProductAttributes)
                .HasForeignKey(pa => pa.AttributeId);

            builder.Property(pa => pa.CreatedBy).IsRequired();
            builder.Property(pa => pa.UpdatedBy).IsRequired();

            builder.Property(pa => pa.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(pa => pa.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}