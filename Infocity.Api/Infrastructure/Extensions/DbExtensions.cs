using Infocity.Api.Dal.DbContexts;
using Infocity.Api.Domain.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infocity.Api.Infrastructure.Extensions;

public static class DbExtensions
{
    public static void AddDb(this IServiceCollection services, DatabaseConfiguration configuration)
    {
        services.AddDbContext<InfocityDbContext>(options => options
            .UseSqlite($"Data Source={AppDomain.CurrentDomain.BaseDirectory}/{configuration.DbName}.db"));
    }
}