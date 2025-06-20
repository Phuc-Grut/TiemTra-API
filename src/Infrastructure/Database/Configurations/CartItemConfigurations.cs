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
    internal class CartItemConfigurations : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");

            // Khóa chính       
            builder.HasKey(ci => ci.CartItemId);

            // Thiết lập quan hệ với Cart
            builder.HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItem)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // Thiết lập quan hệ với Product    

            builder.HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()")
               .ValueGeneratedOnAdd();

            builder.Property(c => c.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
