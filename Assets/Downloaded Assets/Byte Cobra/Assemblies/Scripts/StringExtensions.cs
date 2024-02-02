namespace ByteCobra.Assemblies
{
    public static class StringExtensions
    {
        public static string GetStringBetween(this string input, string start, string end)
        {
            int startIndex = input.IndexOf(start);
            int endIndex = input.IndexOf(end, startIndex + start.Length);

            if (startIndex >= 0 && endIndex >= 0)
            {
                int length = endIndex - (startIndex + start.Length);
                return input.Substring(startIndex + start.Length, length);
            }

            return null;
        }
    }
}