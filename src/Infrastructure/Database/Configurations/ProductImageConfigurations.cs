using Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class ProductImageConfigurations : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");

            builder.HasKey(pi => pi.ProductImageId);

            builder.Property(pi => pi.ProductId)
               .IsRequired(false);

            builder.HasOne(pi => pi.Product)
               .WithMany(p => p.ProductImages)
               .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(pi => pi.CreatedBy).IsRequired();
            builder.Property(pi => pi.UpdatedBy).IsRequired();

            builder.Property(pi => pi.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(pi => pi.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}