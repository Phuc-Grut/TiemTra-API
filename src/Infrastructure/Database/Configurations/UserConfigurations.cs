using Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(us => us.UserId);
            builder.HasIndex(us => us.Email);

            builder.Property(us => us.CreatedBy).IsRequired();
            builder.Property(us => us.UpdatedBy).IsRequired();

            builder.Property(us => us.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(us => us.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();

            builder.HasMany(u => u.UserRoles)
           .WithOne(ur => ur.User)
           .HasForeignKey(ur => ur.UserId);
        }
    }
}