using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ByteCobra.Assemblies
{
    public class AssemblyDefinitionGenerator
    {
        public void GenerateAssembly(string assemblyDefName, string assemblyDefPath,
            string nameSpace = null,
            IEnumerable<string> references = null,
            IEnumerable<Platform> includePlatforms = null,
            IEnumerable<string> preCompiledReferences = null,
            IEnumerable<string> versionDefines = null)
        {
            using (StreamWriter streamWriter = new StreamWriter(assemblyDefPath))
            {
                streamWriter.WriteLine("{");
                streamWriter.WriteLine($"  \"name\": \"{assemblyDefName}\",");
                streamWriter.WriteLine($"  \"rootNamespace\": \"{(nameSpace != null ? nameSpace : assemblyDefName)}\",");

                Add(streamWriter, "references", references);
                Add(streamWriter, "includePlatforms", includePlatforms?.Select(x => x.ToDescriptionString()));

                if (preCompiledReferences != null && preCompiledReferences.Count() > 0)
                {
                    streamWriter.WriteLine("\"overrideReferences\": true, ");
                    Add(streamWriter, "precompiledReferences", preCompiledReferences);
                }

                streamWriter.WriteLine($"  \"allowUnsafeCode\": \"{"true"}\",");

                Add(streamWriter, "versionDefines", versionDefines);
                streamWriter.WriteLine($"  \"noEngineReferences\": false");
                streamWriter.WriteLine("}");
            }
            Debug.Log("<color=cyan>Generated assembly at:</color><color=white> " + assemblyDefPath + "</color>");

            AssemblyInfoManager infoManager = new AssemblyInfoManager();
            string directory = Directory.GetParent(assemblyDefPath).FullName;
            infoManager.Create(directory);
        }

        private void Add(StreamWriter streamWriter, string name, IEnumerable<string> values = null, bool addComma = true)
        {
            if (values == null)
            {
                streamWriter.WriteLine($"  \"{name}\": []{(addComma ? "," : string.Empty)}");
                return;
            }

            streamWriter.WriteLine($"  \"{name}\": [");

            if (values != null && values.Count() > 0)
            {
                for (int i = 0; i < values.Count(); i++)
                {
                    if (i < values.Count() - 1)
                        streamWriter.WriteLine($"    \"{values.ElementAt(i)}\",");
                    else
                        streamWriter.WriteLine($"    \"{values.ElementAt(i)}\"");
                }
            }

            streamWriter.WriteLine($" ]{(addComma ? "," : string.Empty)}");
        }
    }
}