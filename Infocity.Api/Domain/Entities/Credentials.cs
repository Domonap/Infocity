using Newtonsoft.Json;

namespace Infocity.Api.Domain.Entities;

public class Credentials
{
    public string Password { get; set; }
    public string Salt { get; set; }

    [JsonIgnore] public virtual string Username { get; set; }

    [JsonIgnore] public virtual User User { get; set; }
}