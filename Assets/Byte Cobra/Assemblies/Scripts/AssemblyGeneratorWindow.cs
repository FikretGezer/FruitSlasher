#if !UNITY_EDITOR
#else

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ByteCobra.Assemblies
{
    public class AssemblyGeneratorWindow : EditorWindow
    {
        [SerializeField]
        protected VisualTreeAsset baseVisualTree;

        [SerializeField]
        protected VisualTreeAsset excludedDirTree;

        [SerializeField]
        protected Texture2D Icon;

        private TemplateContainer tc;

        private static string s_directory = string.Empty;
        private static string s_excludedDirectory = string.Empty;

        private TextField directoryElement;
        private TextField excludedDirectoryElement;

        private string originalExcludeDirTooltip = string.Empty;

        private List<ExcludedDirectoryElement> excludedFiles = new List<ExcludedDirectoryElement>();
        private Foldout excludedDirsFoldout;

        [MenuItem("Tools/Byte Cobra/Assemblies")]
        private static void ShowWindow()
        {
            GetWindow<AssemblyGeneratorWindow>("Cobra Assemblies");
        }

        private void OnGUI()
        {
            if (tc == null)
            {
                tc = baseVisualTree.Instantiate();
                rootVisualElement.Add(baseVisualTree.Instantiate());
                titleContent = new GUIContent("Cobra Assemblies", Icon);
                excludedDirsFoldout = rootVisualElement.Query<Foldout>("excluded-directories-folodout");
                excludedDirsFoldout.Clear();

                var rawExcludedPaths = EditorPrefs.GetString(nameof(excludedFiles), "");

                if (!string.IsNullOrEmpty(rawExcludedPaths))
                {
                    List<string> formattedExcludedPaths = rawExcludedPaths.FromCommaSeparatedString().ToList();
                    foreach (string path in formattedExcludedPaths)
                    {
                        var element = new ExcludedDirectoryElement(excludedDirTree);
                        element.TextField.SetValueWithoutNotify(path);
                        excludedFiles.Add(element);
                        excludedDirsFoldout.Add(element.Root);
                        element.DeleteButton.clicked += () => OnDeleteFieldClicked(element);
                    }
                }
                else
                {
                    var element = new ExcludedDirectoryElement(excludedDirTree);
                    excludedFiles.Add(element);
                    excludedDirsFoldout.Add(element.Root);
                    element.DeleteButton.clicked += () => OnDeleteFieldClicked(element);
                }

                ConnectButton(rootVisualElement, "generate-button", () => OnCreateClicked(rootVisualElement));
                ConnectButton(rootVisualElement, "clear-button", () => OnDeleteClicked(rootVisualElement));
                ConnectButton(rootVisualElement, "clear-excluded-button", () => OnClearExcludedClicked());
                ConnectButton(rootVisualElement, "add-directory-button", () => OnAddExcludedDirectoryClicked());
            }
        }

        private void OnAddExcludedDirectoryClicked()
        {
            var element = new ExcludedDirectoryElement(excludedDirTree);
            excludedFiles.Add(element);
            excludedDirsFoldout.Add(element.Root);
            element.DeleteButton.clicked += () => OnDeleteFieldClicked(element);
            EditorPrefs.SetString(nameof(excludedFiles), excludedFiles.Select(x => x.TextField.text).ToCommaSeparatedString());
        }

        private void OnDeleteFieldClicked(ExcludedDirectoryElement element)
        {
            if (excludedFiles.Count > 1)
            {
                excludedFiles.Remove(element);
                excludedDirsFoldout.Remove(element.Root);
            }
        }

        private void OnClearExcludedClicked()
        {
            excludedDirsFoldout.Clear();
            excludedFiles.Clear();

            var element = new ExcludedDirectoryElement(excludedDirTree);
            excludedFiles.Add(element);
            excludedDirsFoldout.Add(element.Root);
            element.DeleteButton.clicked += () => OnDeleteFieldClicked(element);

            EditorPrefs.SetString(nameof(excludedFiles), excludedFiles.Select(x => x.TextField.text).ToCommaSeparatedString());
        }

        private void OnCreateClicked(VisualElement element)
        {
            ModuleGenerator generator = new ModuleGenerator();
            generator.GenerateModule(Path.GetFullPath(Application.dataPath), excludedFiles.Select(x => x.TextField.text).ToList());
        }

        private void OnDeleteClicked(VisualElement element)
        {
            ProjectAssemblyManager manager = new ProjectAssemblyManager();
            manager.Restore();
        }

        private void ConnectButton(VisualElement element, string name, Action OnClickAction)
        {
            Button button = element.Query<Button>(name);
            if (button != null)
                button.clicked += () => OnClickAction();
        }

        private bool GetDirectories()
        {
            s_directory = directoryElement.text;
            s_excludedDirectory = excludedDirectoryElement.text;

            if (string.IsNullOrEmpty(s_directory))
                return false;

            try
            {
                string fullDirectory = Path.GetFullPath(s_directory);
                if (!Directory.Exists(s_directory))
                {
                    Debug.LogError("Directory doesn't exists: " + fullDirectory);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message + "\n" + s_directory);
                return false;
            }

            if (!string.IsNullOrEmpty(s_excludedDirectory))
            {
                try
                {
                    string fullExcludedDirectory = Path.GetFullPath(s_directory);
                    if (!Directory.Exists(s_excludedDirectory))
                    {
                        Debug.LogError("Directory doesn't exists: " + fullExcludedDirectory);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message + "\n" + s_excludedDirectory);
                    return false;
                }
            }

            return true;
        }

        public void OpenWebPage(string url)
        {
            Application.OpenURL(url);
        }
    }
}

#endif