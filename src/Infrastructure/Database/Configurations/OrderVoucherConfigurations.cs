using Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class OrderVoucherConfigurations : IEntityTypeConfiguration<OrderVoucher>
    {
        public void Configure(EntityTypeBuilder<OrderVoucher> builder)
        {
            builder.ToTable("OrderVouchers");

            builder.HasKey(ov => ov.OrderVoucherId);

            // Composite unique index để tránh trùng lặp Order-Voucher
            builder.HasIndex(ov => new { ov.OrderId, ov.VoucherId })
                .IsUnique();

            builder.Property(ov => ov.VoucherCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(ov => ov.DiscountAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(ov => ov.UsedAt)
                .IsRequired();

            // Relationships
            builder.HasOne(ov => ov.Order)
                .WithMany(o => o.OrderVouchers)
                .HasForeignKey(ov => ov.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ov => ov.Voucher)
                .WithMany(v => v.OrderVouchers)
                .HasForeignKey(ov => ov.VoucherId)
                .OnDelete(DeleteBehavior.Restrict);

            // Base entity properties
            builder.Property(ov => ov.CreatedBy).IsRequired();
            builder.Property(ov => ov.UpdatedBy).IsRequired();

            builder.Property(ov => ov.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(ov => ov.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}