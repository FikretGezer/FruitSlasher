using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ByteCobra.Assemblies
{
    public class DirectoryScanner
    {
        private List<DirectoryInfo> GetSubDirectories(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            var subDirectories = directory.GetDirectories("*.*", SearchOption.AllDirectories).ToList();
            return subDirectories;
        }

        public List<DirectoryInfo> GetRuntimeDirectories(string path)
        {
            var subDirectories = GetSubDirectories(path);
            List<DirectoryInfo> filteredDirs = new List<DirectoryInfo>();

            foreach (var subDirectory in subDirectories)
            {
                if (ContainsFile(subDirectory, ".genasm"))
                    continue;

                var directories = GetDirectoryNames(subDirectory.FullName);
                if (directories.Any(x => string.Equals(x, "Editor", StringComparison.Ordinal)))
                    continue;
                else if (directories.Any(x => string.Equals(x, "Assembly Generator", StringComparison.Ordinal)))
                    continue;
                else
                {
                    var platforms = AssemblyReader.GetAssemblyPlatforms(subDirectory.FullName);
                    bool editorPlatform = platforms.Count == 1 && string.Equals(platforms.First(), "Editor", StringComparison.Ordinal);
                    if (editorPlatform)
                        continue;
                }

                bool containsAsmdef = ContainsFile(subDirectory, "asmdef");
                bool containsScript = ContainsFile(subDirectory, "cs");

                if (!containsAsmdef && !containsScript)
                    continue;

                filteredDirs.Add(subDirectory);
            }

            DirectoryInfo parent = new DirectoryInfo(path);
            if (parent.Name == "Editor")
                return filteredDirs;
            else
            {
                if (ContainsFile(parent, ".genasm"))
                    return filteredDirs;

                filteredDirs.Add(new DirectoryInfo(path));
                return filteredDirs;
            }
        }

        public List<DirectoryInfo> GetEditorDirectories(string path)
        {
            var subDirectories = GetSubDirectories(path);

            List<DirectoryInfo> filteredDirs = new List<DirectoryInfo>();

            foreach (var subDirectory in subDirectories)
            {
                if (ContainsFile(subDirectory, ".genasm"))
                    continue;

                var directories = GetDirectoryNames(subDirectory.FullName);
                if (directories.Any(x => string.Equals(x, "Editor", StringComparison.Ordinal)))
                {
                    bool containsAsmdef = ContainsFile(subDirectory, "asmdef");
                    bool containsScript = ContainsFile(subDirectory, "cs");

                    if (!containsAsmdef && !containsScript)
                        continue;

                    filteredDirs.Add(subDirectory);
                }
                else if (directories.Any(x => string.Equals(x, "Assembly Generator", StringComparison.Ordinal)))
                    continue;
                else
                {
                    var platforms = AssemblyReader.GetAssemblyPlatforms(subDirectory.FullName);
                    bool editorPlatform = platforms.Count == 1 && string.Equals(platforms.First(), "Editor", StringComparison.Ordinal);
                    if (editorPlatform)
                    {
                        bool containsAsmdef = ContainsFile(subDirectory, ".asmdef");
                        bool containsScript = ContainsFile(subDirectory, "cs");

                        if (!containsAsmdef && !containsScript)
                            continue;

                        filteredDirs.Add(subDirectory);
                    }
                }
            }

            DirectoryInfo parent = new DirectoryInfo(path);
            if (!string.Equals(parent.Name, "Editor", StringComparison.Ordinal))
                return filteredDirs;
            else
            {
                if (ContainsFile(parent, ".genasm"))
                    return filteredDirs;

                filteredDirs.Add(new DirectoryInfo(path));
                return filteredDirs;
            }
        }

        public static bool ContainsFile(DirectoryInfo subDirectory, string extension)
        {
            var files = subDirectory.GetFiles();
            bool containsFile = files.Any(x => x.Extension.EndsWith(extension));
            return containsFile;
        }

        public static List<string> GetDirectoryNames(string path)
        {
            List<string> folderNames = new List<string>();

            // Split the path into its components using the directory separator character.
            string[] components = path.Split('\\');

            // Add each non-empty component to the folderNames list.
            foreach (string component in components)
            {
                if (!string.IsNullOrEmpty(component))
                {
                    folderNames.Add(component);
                }
            }

            return folderNames;
        }
    }
}