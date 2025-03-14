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
    public class CategoryAttributeConfigurations : IEntityTypeConfiguration<CategoryAttribute>
    {
        public void Configure(EntityTypeBuilder<CategoryAttribute> builder)
        {
            builder.ToTable("CategoryAttributes");

            builder.HasKey(ca => ca.CategoryAttributeId);

            builder.HasOne(ca => ca.Category)
               .WithMany(c => c.CategoryAttributes)
               .HasForeignKey(ca => ca.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ca => ca.Attribute)
              .WithMany(a => a.CategoryAttributes)
              .HasForeignKey(ca => ca.AttributeId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Property(ca => ca.CategoryId)
              .IsRequired();

            builder.Property(ca => ca.AttributeId)
                   .IsRequired();

            builder.Property(ca => ca.CreatedBy).IsRequired();
            builder.Property(ca => ca.UpdatedBy).IsRequired();

            builder.Property(ca => ca.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(ca => ca.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
