using System.Collections.Generic;
using System.IO;

namespace ByteCobra.Assemblies
{
    public class ScriptDirectoryScanner
    {
        /// <summary>
        /// Returns all scripts that dont have asmdefs or asmrefs.
        /// </summary>
        public IEnumerable<string> Scan(string path)
        {
            var validDirectories = new List<string>();
            DirectoryInfo directory = new DirectoryInfo(path);

            // Get all subdirectories
            var subDirectories = directory.GetDirectories();

            // Check each subdirectory for a .cs file and no .asmdef or .asmref files
            foreach (var subDirectory in subDirectories)
            {
                var hasCsFile = false;
                var hasAsmDefFile = false;
                var hasAsmRefFile = false;

                // Check for .cs files
                var files = subDirectory.GetFiles();
                foreach (var file in files)
                {
                    if (file.Extension == ".cs")
                    {
                        hasCsFile = true;
                    }
                    if (file.Extension == ".asmdef")
                    {
                        hasAsmDefFile = true;
                    }
                    if (file.Extension == ".asmref")
                    {
                        hasAsmRefFile = true;
                    }
                }

                // If the subdirectory has a .cs file and no .asmdef or .asmref files, add it to the list
                if (hasCsFile && !hasAsmDefFile && !hasAsmRefFile)
                {
                    validDirectories.Add(subDirectory.FullName);
                }
            }

            return validDirectories;
        }
    }
}