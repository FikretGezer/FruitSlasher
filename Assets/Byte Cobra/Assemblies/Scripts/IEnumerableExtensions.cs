using System.Collections.Generic;
using System.Linq;

namespace ByteCobra.Assemblies
{
    public static class IEnumerableExtensions
    {
        public static string ToCommaSeparatedString<T>(this IEnumerable<T> items)
        {
            if (items == null)
                return "null";
            if (items.Count() == 0)
                return "";

            return string.Join(", ", items);
        }

        public static IEnumerable<string> FromCommaSeparatedString(this string str)
        {
            return str.Split(',')
                .Select(s => s.Trim());
        }
    }
}