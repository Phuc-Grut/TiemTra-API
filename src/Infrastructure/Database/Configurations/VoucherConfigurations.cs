using Domain.Data.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Configurations
{
    public class VoucherConfigurations : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.ToTable("Vouchers");

            builder.HasKey(v => v.VoucherId);

            builder.Property(v => v.VoucherCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(v => v.VoucherCode)
                .IsUnique();

            builder.Property(v => v.VoucherName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(v => v.Description)
                .HasMaxLength(500);

            builder.Property(v => v.Quantity)
                .IsRequired();

            builder.Property(v => v.UsedQuantity)
                .HasDefaultValue(0);

            builder.Property(v => v.DiscountPercentage)
                .IsRequired()
                .HasColumnType("decimal(5,2)"); // 0.01% đến 100%

            builder.Property(v => v.EndDate)
                .IsRequired();

            builder.Property(v => v.Status)
                .IsRequired()
                .HasDefaultValue(VoucherStatus.Pending);

            // Relationships
            builder.HasOne(v => v.Creator)
                .WithMany()
                .HasForeignKey(v => v.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(v => v.Updater)
                .WithMany()
                .HasForeignKey(v => v.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Base entity properties
            builder.Property(v => v.CreatedBy).IsRequired();
            builder.Property(v => v.UpdatedBy).IsRequired();

            builder.Property(v => v.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(v => v.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}