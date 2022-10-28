using Infocity.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infocity.Api.Dal.EntityConfigurations;

public class CredentialConfiguration : IEntityTypeConfiguration<Credentials>
{
    #region IEntityTypeConfiguration<Credentials> Members

    public void Configure(EntityTypeBuilder<Credentials> builder)
    {
        builder.Property(x => x.Username);

        builder.HasKey(x => x.Username);

        builder.Property(x => x.Password)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithOne(x => x.Credentials)
            .HasForeignKey<Credentials>(x => x.Username);
    }

    #endregion
}