#region

using System.Text.RegularExpressions;

#endregion

namespace Infocity.Api.Infrastructure.Helpers
{
    public static class RegexConstants
    {
        //  public static readonly Regex NicknameOneChar = new Regex("[а-яА-Яa-zA-Z]+");
        public static readonly Regex NicknameOneChar = new("[A-Za-z0-9]+");

        /// <summary>
        ///     Никнейм может состоять только из букв и цифр, а также содержать хотя бы одну букву
        /// </summary>
        public static readonly Regex NicknameRegex = new("(^[a-zA-Z0-9]{3,25}$)");
    }
}