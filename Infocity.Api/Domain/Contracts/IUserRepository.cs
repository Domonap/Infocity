using Infocity.Api.Domain.Entities;

namespace Infocity.Api.Domain.Contracts;

public interface IUserRepository
{
    Task<User> GetUser(string username);
    Task<User> UpdateUser(User sourceUser);
    Task<User> AddUser(User user);
    Task<IList<string>> GetRegisteredUsernames();
}