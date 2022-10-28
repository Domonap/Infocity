using Infocity.Api.Dal.DbContexts;
using Infocity.Api.Domain.Configurations;
using Infocity.Api.Domain.Contracts;
using Infocity.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Infocity.Api.Dal.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    private readonly IMemoryCache _memoryCache;

    public UserRepository(InfocityDbContext context, IOptions<DatabaseConfiguration> configuration, IMemoryCache memoryCache)
        : base(context, configuration)
    {
        _memoryCache = memoryCache;
    }


    public async Task<User> GetUser(string username)
    {
        return await Context.Users
            .Include(x => x.Credentials)
            .SingleOrDefaultAsync(x => x.Username == username);
    }


    public async Task<User> UpdateUser(User sourceUser)
    {
        var entry = Context.Users.Update(sourceUser);
        await Context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<User> AddUser(User user)
    {
        var entry = await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<IList<string>> GetRegisteredUsernames()
    {
        var usernames = _memoryCache.Get<List<string>>("_usernames");

        if (usernames != null) return usernames;

        usernames = await Task.FromResult(Context.Users
            .Select(x => x.Username)
            .IgnoreQueryFilters()
            .OrderBy(x => x)
            .ToList());


        _memoryCache.Set("_usernames", usernames, TimeSpan.FromMinutes(30));

        return usernames;
    }
}