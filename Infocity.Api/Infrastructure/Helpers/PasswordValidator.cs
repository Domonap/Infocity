using System.Text.RegularExpressions;

namespace Infocity.Api.Infrastructure.Helpers;

public static class PasswordValidator
{
    public static bool IsValid(string password)
    {
        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasMinimum8Chars = new Regex(@".{6,}");
        var isValidated = hasNumber.IsMatch(password)
                          && hasUpperChar.IsMatch(password)
                          && hasMinimum8Chars.IsMatch(password);

        return isValidated;
    }
}