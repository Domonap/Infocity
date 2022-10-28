using Infocity.Api.Domain.Entities;
using Infocity.Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infocity.Api.Dal.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Username)
            .HasName("PK_User");

        builder.Property(e => e.Username)
            .HasColumnType("VARCHAR(100)")
            .HasMaxLength(100)
            .HasConversion(v => v.ToLowerInvariant(),
                v => v);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.UserStatus)
            .HasDefaultValue(UserStatus.Active);


        builder.HasQueryFilter(x => x.UserStatus != UserStatus.Deactivated);
    }
}