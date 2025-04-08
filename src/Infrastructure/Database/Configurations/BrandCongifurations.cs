using Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class BrandCongifurations : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brands");

            builder.HasKey(b => b.BrandId);

            builder.HasMany(b => b.Products)
                            .WithOne(p => p.Brand)
                            .HasForeignKey(p => p.BrandId)
                            .OnDelete(DeleteBehavior.SetNull);

            builder.Property(b => b.CreatedBy).IsRequired();
            builder.Property(b => b.UpdatedBy).IsRequired();

            builder.Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(b => b.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}