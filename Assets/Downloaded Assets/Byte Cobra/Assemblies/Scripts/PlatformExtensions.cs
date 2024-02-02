using System.ComponentModel;

namespace ByteCobra.Assemblies
{
    public static class PlatformExtensions
    {
        public static string ToDescriptionString(this Platform val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}