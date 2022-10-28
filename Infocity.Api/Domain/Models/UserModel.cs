using Infocity.Api.Domain.Enums;

namespace Infocity.Api.Domain.Models;

public class UserModel
{
    public string Username { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
    public UserStatus UserStatus { get; set; }
}