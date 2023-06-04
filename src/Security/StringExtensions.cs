namespace food_detective.Security
{
    public static class StringExtensions
    {
        public static string StripHtml = "<.*?>";
        public static string StripUrl = @"(https?://\S+|www\.\S+)";
    }
}
