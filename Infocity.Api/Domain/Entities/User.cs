using Infocity.Api.Domain.Enums;
using Newtonsoft.Json;

namespace Infocity.Api.Domain.Entities;

public class User
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserStatus UserStatus { get; set; }

    [JsonIgnore] public virtual Credentials Credentials { get; set; }
}