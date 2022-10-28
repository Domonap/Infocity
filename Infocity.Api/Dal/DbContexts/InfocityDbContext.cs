using Infocity.Api.Dal.EntityConfigurations;
using Infocity.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infocity.Api.Dal.DbContexts;

public sealed class InfocityDbContext : DbContext
{
    public InfocityDbContext(DbContextOptions options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CredentialConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }

    public DbSet<User> Users { get; set; }
}