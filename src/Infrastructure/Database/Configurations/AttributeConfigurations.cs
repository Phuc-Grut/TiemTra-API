using Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class AttributeConfigurations : IEntityTypeConfiguration<Attributes>
    {
        public void Configure(EntityTypeBuilder<Attributes> builder)
        {
            // Tên bảng
            builder.ToTable("Attributes");

            // Khóa chính
            builder.HasKey(a => a.AttributeId);

            builder.HasMany(a => a.ProductAttributes)
                   .WithOne(pa => pa.Attribute)
                   .HasForeignKey(pa => pa.AttributeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(a => a.CreatedBy).IsRequired();
            builder.Property(a => a.UpdatedBy).IsRequired();

            builder.Property(a => a.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(a => a.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}