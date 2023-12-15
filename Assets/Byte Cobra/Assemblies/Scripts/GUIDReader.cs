using System.IO;

namespace ByteCobra.Assemblies
{
    public class GUIDReader
    {
        public string ReadGuidFromMeta(string filePath)
        {
            string guid = "";
            string metaPath = filePath + ".meta";

            if (File.Exists(metaPath))
            {
                using (StreamReader reader = new StreamReader(metaPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("guid:"))
                        {
                            guid = line.Substring(6);
                            break;
                        }
                    }
                }
            }
            return guid;
        }
    }
}