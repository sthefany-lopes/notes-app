namespace Notes.Extensions
{
    public static class StringExtensions
    {
        public static string Cut (this string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str) || str.Length <= maxLength)
                return str;

            return str.Substring(0, maxLength) + "...";
        }
    }
}