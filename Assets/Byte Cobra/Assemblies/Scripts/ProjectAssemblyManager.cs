using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ByteCobra.Assemblies
{
    public class ProjectAssemblyManager
    {
        public ScriptDirectoryScanner ScriptDirectoryScanner { get; private set; }
        protected string AssetsFolder => Application.dataPath;
        protected string GeneratedAssembliesFolder => Path.Combine(Application.dataPath, "Zappy", "Assembly Generator", "Generated Assemblies");
        protected string GeneratedEditorAssembliesFolder => Path.Combine(GeneratedAssembliesFolder, "Editor");
        protected string GeneratedRuntimeAssembliesFolder => Path.Combine(GeneratedAssembliesFolder, "Runtime");
        protected string EditorAssemblyPath => Path.Combine(GeneratedEditorAssembliesFolder, "AssemblyGenerator.Generated.Editor.asmdef");
        protected string EditorAssemblyMetaPath => Path.Combine(GeneratedEditorAssembliesFolder, "AssemblyGenerator.Generated.Editor.asmdef.meta");
        protected string RuntimeAssemblyPath => Path.Combine(GeneratedRuntimeAssembliesFolder, "AssemblyGenerator.Generated.Runtime.asmdef");
        protected string RuntimeAssemblyMetaPath => Path.Combine(GeneratedRuntimeAssembliesFolder, "AssemblyGenerator.Generated.Runtime.asmdef.meta");

        public void GenerateAssemblies(string excludedDir = "", HashSet<Platform> runtimePlatforms = null)
        {
            if (!Directory.Exists(GeneratedAssembliesFolder))
                Directory.CreateDirectory(GeneratedAssembliesFolder);

            if (!Directory.Exists(GeneratedEditorAssembliesFolder))
                Directory.CreateDirectory(GeneratedEditorAssembliesFolder);

            if (!Directory.Exists(GeneratedRuntimeAssembliesFolder))
                Directory.CreateDirectory(GeneratedRuntimeAssembliesFolder);

            AssemblyDefinitionGenerator assemblyDefinitionGenerator = new AssemblyDefinitionGenerator();
            List<Platform> editorPlatforms = new List<Platform>();
            editorPlatforms.Add(Platform.Editor);

            List<string> runtimeReferences = new List<string>();
            List<string> runtimePreCompiledReferences = new List<string>();

            List<string> editorReferences = new List<string>();
            List<string> editorPreCompiledReferences = new List<string>();

            string path = Application.dataPath;

            DirectoryScanner dirScanner = new DirectoryScanner();
            var runtimeDirectories = dirScanner.GetRuntimeDirectories(path);
            var editorDirectories = dirScanner.GetEditorDirectories(path);

            if (!string.IsNullOrEmpty(excludedDir))
            {
                DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(excludedDir));
                runtimeDirectories.RemoveAll(x =>
                {
                    var currDir = x.FullName;
                    return currDir.Contains(excludedDir);
                });

                editorDirectories.RemoveAll(x =>
                {
                    var currDir = x.FullName;
                    return currDir.Contains(excludedDir);
                });
            }

            foreach (var directory in new List<DirectoryInfo>(runtimeDirectories))
            {
                var references = AssemblyReader.GetAssemblyReferences(directory.FullName);
                var preCompiledReferences = AssemblyReader.GetPreCompiledReferences(directory.FullName);
                runtimeReferences.AddRange(references);
                runtimePreCompiledReferences.AddRange(preCompiledReferences);
            }

            foreach (var directory in editorDirectories)
            {
                editorReferences.AddRange(AssemblyReader.GetAssemblyReferences(directory.FullName));
                editorPreCompiledReferences.AddRange(AssemblyReader.GetPreCompiledReferences(directory.FullName));
            }

            editorReferences.Add("AssemblyGenerator.Generated.Runtime");

            HashSet<string> uniqueRuntimeReferences = new HashSet<string>(runtimeReferences);
            HashSet<string> uniqueRuntimePreCompiledReferences = new HashSet<string>(runtimePreCompiledReferences);

            HashSet<string> uniqueEditorReferences = new HashSet<string>(editorReferences);
            HashSet<string> uniqueEditorPreCompiledReferences = new HashSet<string>(editorPreCompiledReferences);

            if (uniqueRuntimeReferences.Contains("AssemblyGenerator"))
                uniqueRuntimeReferences.Remove("AssemblyGenerator");

            if (uniqueEditorReferences.Contains("AssemblyGenerator"))
                uniqueEditorReferences.Remove("AssemblyGenerator");

            assemblyDefinitionGenerator.GenerateAssembly("AssemblyGenerator.Generated.Runtime", RuntimeAssemblyPath, includePlatforms: runtimePlatforms, references: uniqueRuntimeReferences, preCompiledReferences: uniqueRuntimePreCompiledReferences);
            assemblyDefinitionGenerator.GenerateAssembly("AssemblyGenerator.Generated.Editor", EditorAssemblyPath, includePlatforms: editorPlatforms, references: uniqueEditorReferences, preCompiledReferences: uniqueEditorPreCompiledReferences);

            AssetDatabase.Refresh();

            Generate(runtimeDirectories.Select(x => x.FullName), editorDirectories.Select(x => x.FullName));

            AssetDatabase.Refresh();
        }

        private void Generate(IEnumerable<string> runtimeDirectories, IEnumerable<string> editorDirectories)
        {
            //Restore();

            AssemblyReplacer assemblyReplacer = new AssemblyReplacer();
            AssemblyReferenceGenerator asmRefGenerator = new AssemblyReferenceGenerator();

            foreach (var directory in runtimeDirectories)
            {
                assemblyReplacer.Replace(directory, ".asmdef");
                asmRefGenerator.GenerateAssemblyReference(Path.Combine(directory, "Reference.asmref"), RuntimeAssemblyPath);
            }

            foreach (var directory in editorDirectories)
            {
                assemblyReplacer.Replace(directory, ".asmdef");
                asmRefGenerator.GenerateAssemblyReference(Path.Combine(directory, "Reference.asmref"), EditorAssemblyPath);
            }

            AssetDatabase.Refresh();
        }

        public void Restore()
        {
            AssemblyRestorer restorer = new AssemblyRestorer();
            restorer.Restore(Application.dataPath);

            string generatedAsmFolderMetaPath = GeneratedAssembliesFolder + ".meta";

            if (Directory.Exists(GeneratedAssembliesFolder))
                Directory.Delete(GeneratedAssembliesFolder, true);

            if (File.Exists(generatedAsmFolderMetaPath))
                File.Delete(generatedAsmFolderMetaPath);

            AssetDatabase.Refresh();
        }
    }
}