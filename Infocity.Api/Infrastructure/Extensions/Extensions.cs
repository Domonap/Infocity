using System.Net.Http.Headers;
using System.Text;

namespace Infocity.Api.Infrastructure.Extensions;

public static class Extensions
{
    public static string GetUsername(this HttpRequest request)
    {
        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] {':'}, 2);
            var username = credentials[0];
            return username;
        }
        catch (Exception)
        {
            return null;
        }
    }
}