using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ByteCobra.Assemblies
{
    public class AssemblyReader
    {
        public static HashSet<string> GetAssemblyReferences(string directory)
        {
            HashSet<string> result = new HashSet<string>();

            directory = Path.GetFullPath(directory);
            var files = Directory.GetFiles(directory);
            var asmDefFile = files.FirstOrDefault(x => x.EndsWith("asmdef"));
            string fileContent = null;
            if (asmDefFile != null)
            {
                fileContent = File.ReadAllText(asmDefFile);
            }
            else return result;

            var filteredContent = fileContent.Replace(" ", "").Replace("\n", "");
            var match = fileContent.IndexOf("precompiledReferences:");
            var substr = filteredContent.GetStringBetween("references\":[", "]");
            if (string.IsNullOrEmpty(substr))
                return result;

            var items = substr.Split(',');

            foreach (var item in items)
            {
                var filteredItem = item.Replace("\"", "");
                if (filteredItem.Contains("AssemblyGenerator"))
                    continue;

                if (!string.IsNullOrEmpty(filteredItem))
                    result.Add(Regex.Replace(filteredItem, @"\s+", string.Empty));
            }

            return result;
        }

        public static HashSet<string> GetPreCompiledReferences(string directory)
        {
            HashSet<string> result = new HashSet<string>();

            directory = Path.GetFullPath(directory);
            var file = Directory.GetFiles(directory).FirstOrDefault(x => x.EndsWith("asmdef"));
            string fileContent = null;
            if (file != null)
            {
                fileContent = File.ReadAllText(file);
            }
            else return result;

            var filteredContent = fileContent.Replace(" ", "").Replace("\n", "");
            var match = fileContent.IndexOf("precompiledReferences:");
            var substr = filteredContent.GetStringBetween("precompiledReferences\":[", "]");
            if (string.IsNullOrEmpty(substr))
                return result;
            var items = substr.Split(',');

            foreach (var item in items)
            {
                var filteredItem = item.Replace("\"", "");
                if (!string.IsNullOrEmpty(filteredItem))
                {
                    result.Add(Regex.Replace(filteredItem, @"\s+", string.Empty));
                }
            }

            return result;
        }

        public static List<string> GetAssemblyPlatforms(string directory)
        {
            List<string> result = new List<string>();
            directory = Path.GetFullPath(directory);
            var file = Directory.GetFiles(directory).FirstOrDefault(x => x.EndsWith(".asmdef"));
            string fileContent = null;
            if (file != null)
            {
                fileContent = File.ReadAllText(file);
            }
            else
            {
                return new List<string>();
            }

            var filteredContent = fileContent.Replace(" ", "").Replace("\n", "");
            var substr = filteredContent.GetStringBetween("includePlatforms\":[", "]");
            if (substr != null)
            {
                var items = substr.Split(',');

                foreach (var item in items)
                {
                    var filteredItem = item.Replace("\"", "");
                    if (!string.IsNullOrEmpty(filteredItem))
                        result.Add(Regex.Replace(filteredItem, @"\s+", string.Empty));
                }
            }
            return result;
        }
    }
}