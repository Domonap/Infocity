using Infocity.Api.Dal.DbContexts;
using Infocity.Api.Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infocity.Api.Dal.Repositories;

public abstract class BaseRepository
{
    protected readonly InfocityDbContext Context;

    protected BaseRepository(InfocityDbContext context, IOptions<DatabaseConfiguration> configuration)
    {
        Context = context;
        Context.Database.SetCommandTimeout(configuration.Value.BaseTimeoutMilliseconds);
    }
}