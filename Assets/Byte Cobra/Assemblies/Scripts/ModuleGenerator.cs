using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace ByteCobra.Assemblies
{
    public class ModuleGenerator
    {
        public void GenerateModule(string directory, List<string> excludedDirs)
        {
            DirectoryScanner dirScanner = new DirectoryScanner();
            var runtimeDirectories = dirScanner.GetRuntimeDirectories(directory);
            var editorDirectories = dirScanner.GetEditorDirectories(directory);

            if (excludedDirs != null)
            {
                foreach (var excludedDir in excludedDirs)
                {
                    if (string.IsNullOrEmpty(excludedDir))
                        continue;

                    DirectoryInfo excludedDirInfo = new DirectoryInfo(excludedDir);

                    if (runtimeDirectories.Contains(excludedDirInfo))
                        runtimeDirectories.Remove(excludedDirInfo);

                    if (editorDirectories.Contains(excludedDirInfo))
                        editorDirectories.Remove(excludedDirInfo);
                }
            }

            Generate(directory, runtimeDirectories.Select(x => x.FullName), editorDirectories.Select(x => x.FullName));
        }

        private void Generate(string rootFolder, IEnumerable<string> runtimeDirectories, IEnumerable<string> editorDirectories)
        {
            string runtimePath = GenerateRuntimeAssmebly(rootFolder);
            string editorPath = GenerateEditorAssembly(rootFolder);

            AssemblyReplacer assemblyReplacer = new AssemblyReplacer();
            AssemblyReferenceGenerator asmRefGenerator = new AssemblyReferenceGenerator();

            List<string> runtimeReferences = new List<string>();
            List<string> runTimePreCompiledReferences = new List<string>();

            List<string> editorReferences = new List<string>();
            List<string> editorPreCompiledReferences = new List<string>();

            var rootFolderPlatforms = AssemblyReader.GetAssemblyPlatforms(rootFolder);

            foreach (var directory in runtimeDirectories)
            {
                runtimeReferences.AddRange(AssemblyReader.GetAssemblyReferences(directory));
                runTimePreCompiledReferences.AddRange(AssemblyReader.GetPreCompiledReferences(directory));

                assemblyReplacer.Replace(directory, ".asmdef");
                assemblyReplacer.Replace(directory, ".asmref");

                asmRefGenerator.GenerateAssemblyReference(Path.Combine(directory, "Reference.asmref"), runtimePath + ".asmdef");
                if (editorDirectories.Contains(directory))
                    throw new Exception("Directory " + directory + " exists in both types");
            }

            foreach (var directory in editorDirectories)
            {
                var references = AssemblyReader.GetAssemblyReferences(directory);
                var preCompiledReferences = AssemblyReader.GetPreCompiledReferences(directory);

                editorReferences.AddRange(references);
                editorPreCompiledReferences.AddRange(preCompiledReferences);

                assemblyReplacer.Replace(directory, ".asmdef");
                assemblyReplacer.Replace(directory, ".asmref");

                asmRefGenerator.GenerateAssemblyReference(directory + "/Reference.asmref", editorPath + ".asmdef");
            }

            HashSet<string> filteredRuntimeReferences = new HashSet<string>(runtimeReferences);
            HashSet<string> filteredRuntimeCompiledReferences = new HashSet<string>(runTimePreCompiledReferences);
            var runtimeAssemblyPath = GenerateRuntimeAssmebly(rootFolder, filteredRuntimeReferences.ToList(), filteredRuntimeCompiledReferences.ToList());

            HashSet<string> filteredEditorReferences = new HashSet<string>(editorReferences);
            HashSet<string> filteredCompiledReferences = new HashSet<string>(editorPreCompiledReferences);

            filteredEditorReferences.Add(Path.GetFileName(runtimeAssemblyPath));
            GenerateEditorAssembly(rootFolder, filteredEditorReferences.ToList(), filteredCompiledReferences.ToList());

            AssetDatabase.Refresh();
        }

        private static string GetAssemblyName(string assemblyDirectory)
        {
            List<DirectoryInfo> folderStructure = GetAllParentDirectories(assemblyDirectory, Application.dataPath).ToList();

            string assemblyPrefix = string.Empty;

            if (folderStructure.Count == 0 || folderStructure == null || folderStructure[0] == null)
                assemblyPrefix = string.Empty;
            else
                assemblyPrefix = folderStructure[0].Name;

            for (int i = 1; i < folderStructure.Count(); i++)
            {
                if (!string.IsNullOrEmpty(assemblyPrefix))
                    assemblyPrefix += "." + folderStructure[i].Name;
                else
                    assemblyPrefix = folderStructure[i].Name;
            }

            assemblyPrefix += "." + new DirectoryInfo(assemblyDirectory).Name;
            assemblyPrefix = Regex.Replace(assemblyPrefix, @"\s+", "");
            return assemblyPrefix;
        }

        private static string GenerateRuntimeAssmebly(string assemblyDirectory, List<string> references = null, List<string> preCompiledReferences = null)
        {
            string assemblyPrefix = string.Empty;

            if (Path.GetFullPath(assemblyDirectory) == Path.GetFullPath(Application.dataPath))
            {
                assemblyPrefix = string.Empty;
            }
            else
            {
                assemblyPrefix = GetAssemblyName(assemblyDirectory);
            }
            string runtimeDirectory = Path.Combine(assemblyDirectory, "Generated Assemblies", "Runtime");

            Debug.Log("Generating runtime in " + runtimeDirectory);

            string runtimeAssemblyName = string.Empty; //assemblyPrefix + ".Runtime";
            if (!string.IsNullOrEmpty(assemblyPrefix))
            {
                runtimeAssemblyName = assemblyPrefix + ".Runtime";
            }
            else
            {
                runtimeAssemblyName = "Runtime";
            }

            string runtimeAssemblyPath = Path.Combine(runtimeDirectory, runtimeAssemblyName);

            if (!Directory.Exists(runtimeDirectory))
                Directory.CreateDirectory(runtimeDirectory);

            AssemblyDefinitionGenerator assemblyDefinitionGenerator = new AssemblyDefinitionGenerator();
            assemblyDefinitionGenerator.GenerateAssembly(runtimeAssemblyName, runtimeAssemblyPath + ".asmdef", references: references, preCompiledReferences: preCompiledReferences);
            return runtimeAssemblyPath;
        }

        private static string GenerateEditorAssembly(string assemblyDirectory, List<string> references = null, List<string> preCompiledReferences = null)
        {
            string assemblyPrefix = string.Empty;

            if (Path.GetFullPath(assemblyDirectory) == Path.GetFullPath(Application.dataPath))
            {
                assemblyPrefix = string.Empty;
            }
            else
            {
                assemblyPrefix = GetAssemblyName(assemblyDirectory);
            }

            string editorDirectory = Path.Combine(assemblyDirectory, "Generated Assemblies", "Editor");
            string editorAssemblyName = string.Empty;

            Debug.Log("Generating editor in " + editorDirectory);

            if (!string.IsNullOrEmpty(assemblyPrefix))
            {
                editorAssemblyName = assemblyPrefix + ".Editor";
            }
            else
            {
                editorAssemblyName = assemblyPrefix + "Editor";
            }
            string editorAssemblyPath = Path.Combine(editorDirectory, editorAssemblyName);
            if (!Directory.Exists(editorDirectory))
                Directory.CreateDirectory(editorDirectory);

            AssemblyDefinitionGenerator assemblyDefinitionGenerator = new AssemblyDefinitionGenerator();

            List<Platform> editorPlatforms = new List<Platform>();
            editorPlatforms.Add(Platform.Editor);

            assemblyDefinitionGenerator.GenerateAssembly(editorAssemblyName, editorAssemblyPath + ".asmdef", includePlatforms: editorPlatforms, references: references, preCompiledReferences: preCompiledReferences);

            AssetDatabase.Refresh();

            return editorAssemblyPath;
        }

        private static IEnumerable<DirectoryInfo> GetAllParentDirectories(string directory, string root)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(directory));
            dir = dir.Parent;

            DirectoryInfo rootDir = new DirectoryInfo(Path.GetFullPath(root));

            Stack<DirectoryInfo> result = new Stack<DirectoryInfo>();

            while (dir.Name != rootDir.Name)
            {
                result.Push(dir);
                dir = dir.Parent;
                if (dir == null)
                    break;
            }

            result.Push(dir);
            return result;
        }
    }
}