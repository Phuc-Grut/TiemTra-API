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
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");

            builder.HasKey(c => c.CategoryId);

            builder.Property(c => c.ParentId)
               .IsRequired(false);

            builder.HasOne(c => c.ParentCategory)  // Mỗi Category có thể có một ParentCategory
               .WithMany(c => c.ChildCategories)  // Một Category có thể có nhiều ChildCategories
               .HasForeignKey(c => c.ParentId)  // Khóa ngoại ParentId
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.Products)
               .WithOne(p => p.Category)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.CategoryAttributes)
              .WithOne(ca => ca.Category)
              .HasForeignKey(ca => ca.CategoryId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.CreatedBy).IsRequired();
            builder.Property(c => c.UpdatedBy).IsRequired();

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
