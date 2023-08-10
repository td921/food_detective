using System.Text.RegularExpressions;

namespace food_detective.Security
{
    public static class StringExtensions
    {
        public static readonly Regex stripHtmlRegex = new ("[«＜].*?[»＞>]|\\s*<[^>]+>\\s*", RegexOptions.Compiled);
        public static readonly Regex stripUrlRegex = new (@"https?://", RegexOptions.Compiled);

        public static string RemoveHtml(this string input)
        {
            return stripHtmlRegex.Replace(input, string.Empty);
        }

        public static string RemoveUrl(this string input)
        {
            string result = stripUrlRegex.Replace(input, string.Empty);
            result = result.Replace(".", ". ");

            return result;
        }
    }
}
