using System.IO;
using UnityEditor;
using UnityEngine;

namespace ByteCobra.Assemblies
{
    public class AssemblyRestorer
    {
        private AssemblyReplacer _replacer = new AssemblyReplacer();

        public void Restore(string directory)
        {
            AssemblyInfoManager infoManager = new AssemblyInfoManager();
            DeleteFiles(directory, infoManager);
            DeleteGeneratedAssembliesFolder(directory);
            AssetDatabase.Refresh();
        }

        private void DeleteFiles(string directory, AssemblyInfoManager infoManager)
        {
            directory = Path.GetFullPath(directory);

            // Get all the files in the current directory
            string[] files = Directory.GetFiles(directory);
            bool genAsmExists = false;
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Extension == ".genasm")
                {
                    genAsmExists = true;
                    break;
                }
            }

            if (genAsmExists)
            {
                string[] asmrefFiles = Directory.GetFiles(directory, "*.asmref", SearchOption.AllDirectories);
                string[] asmGenInfo = Directory.GetFiles(directory, "*.genasm", SearchOption.AllDirectories);
                foreach (string asmrefFile in asmrefFiles)
                {
                    Debug.Log("<color=cyan>Deleted file: " + asmrefFile + "</color>");
                    File.Delete(asmrefFile);
                }
                foreach (string infoFile in asmGenInfo)
                {
                    Debug.Log("<color=cyan>Deleted file: " + infoFile + "</color>");
                    File.Delete(infoFile);
                }
            }

            if (!string.IsNullOrEmpty(directory))
            {
                _replacer.Restore(directory, ".asmdef");
                _replacer.Restore(directory, ".asmref");
            }

            //Get all files in all subdirectories
            string[] subdirectories = Directory.GetDirectories(directory);
            foreach (string subdirectory in subdirectories)
            {
                DeleteFiles(subdirectory, infoManager);
            }
        }

        private void DeleteGeneratedAssembliesFolder(string directory)
        {
            string dirPath = Path.Combine(directory, "Generated Assemblies");
            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath, true);

                string metaPath = dirPath + ".meta";
                File.Delete(metaPath);
            }

            string projectAssembliesFolder = Path.Combine(Application.dataPath, "Zappy", "Assembly Generator", "Generated Assemblies");
            if (Directory.Exists(projectAssembliesFolder))
            {
                Directory.Delete(projectAssembliesFolder, true);
                Debug.Log("<color=cyan>Deleted folder: " + projectAssembliesFolder + "</color>");
            }
        }
    }
}