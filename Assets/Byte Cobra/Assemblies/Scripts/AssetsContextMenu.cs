using System.IO;
using UnityEditor;
using UnityEngine;

namespace ByteCobra.Assemblies
{
    public class AssetsContextMenu
    {
        [MenuItem("Assets/Byte Cobra/Assemblies/Create Module")]
        private static void CreateModule()
        {
            string selectedFolderPath = GetSelectedPath();

            if (!string.IsNullOrEmpty(selectedFolderPath))
            {
                AssemblyRestorer restorer = new AssemblyRestorer();
                restorer.Restore(selectedFolderPath);
                AssetDatabase.Refresh();

                ModuleGenerator generator = new ModuleGenerator();
                generator.GenerateModule(selectedFolderPath, default);
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("Please select a valid folder in the project window. Folder could not be found: " + selectedFolderPath);
            }
        }

        private static string GetSelectedPath()
        {
            string selectedFolderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(selectedFolderPath))
            {
                selectedFolderPath = GetClickedDirFullPath();
            }

            return selectedFolderPath;
        }

        private static string GetClickedDirFullPath()
        {
            string clickedAssetGuid = Selection.assetGUIDs[0];
            string clickedPath = AssetDatabase.GUIDToAssetPath(clickedAssetGuid);
            string clickedPathFull = Path.Combine(Directory.GetCurrentDirectory(), clickedPath);

            FileAttributes attr = File.GetAttributes(clickedPathFull);
            return attr.HasFlag(FileAttributes.Directory) ? clickedPathFull : Path.GetDirectoryName(clickedPathFull);
        }

        [MenuItem("Assets/Byte Cobra/Assemblies/Clean Up Folder")]
        private static void Delete()
        {
            string selectedFolderPath = GetSelectedPath();
            if (!string.IsNullOrEmpty(selectedFolderPath))
            {
                AssemblyRestorer restorer = new AssemblyRestorer();
                restorer.Restore(selectedFolderPath);
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("Please select a valid folder in the project window. Folder could not be found: " + selectedFolderPath);
            }
        }
    }
}