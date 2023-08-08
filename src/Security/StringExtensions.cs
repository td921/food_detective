using System.Text.RegularExpressions;

namespace food_detective.Security
{
    public static class StringExtensions
    {
        public static string StripHtml = "<.*?>";
        public static string StripUrl = @"(https?://\S+|www\.\S+)";

        public static bool StringVerification(string foodName, string? brand)
        {
            foodName = Regex.Replace(foodName, StringExtensions.StripHtml, string.Empty);
            foodName = Regex.Replace(foodName, StringExtensions.StripUrl, string.Empty);

            if (!string.IsNullOrEmpty(brand))
            {
                brand = Regex.Replace(brand, StringExtensions.StripHtml, string.Empty);
                brand = Regex.Replace(brand, StringExtensions.StripUrl, string.Empty);

                if (string.IsNullOrEmpty(brand))
                {
                    return false;
                }
            }

            if (string.IsNullOrEmpty(foodName))
            {
                return false;
            }

            return true;
        }
    }
}
