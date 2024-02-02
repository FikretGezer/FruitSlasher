using System;
using System.IO;
using UnityEngine;

namespace ByteCobra.Assemblies
{
    /// <summary>
    /// Class responsible for generating assembly reference files for Unity.
    /// </summary>
    public class AssemblyReferenceGenerator
    {
        /// <summary>
        /// Generates an assembly reference file at the specified output path, with the given reference assembly meta file path.
        /// </summary>
        /// <param name="outputPath">The path to write the generated assembly reference file to.</param>
        /// <param name="assetpath">The path to the asset file of the assembly to reference.</param>
        public void GenerateAssemblyReference(string outputPath, string assetpath)
        {
            if (File.Exists(outputPath))
            {
                Debug.LogError($"Assembly reference {outputPath} already exists");
                return;
            }

            GUIDReader metaReader = new GUIDReader();
            string referenceGuid = metaReader.ReadGuidFromMeta(assetpath);
            if (string.IsNullOrEmpty(referenceGuid))
            {
                throw new NullReferenceException("Could not read GUID from " + assetpath);
            }

            string referenceLine = $"{{\n    \"reference\": \"GUID:{referenceGuid}\"\n}}";

            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.Write(referenceLine);
            }

            Debug.Log("<color=cyan>Generated assembly reference at:</color><color=white> " + outputPath + "</color>");

            AssemblyInfoManager infoManager = new AssemblyInfoManager();
            string directory = Directory.GetParent(outputPath).FullName;
            infoManager.Create(directory);
        }
    }
}