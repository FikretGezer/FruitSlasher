using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace ByteCobra.Assemblies
{
    /// <summary>
    /// A custom scripted importer that handles importing ".lua" files as text assets.
    /// </summary>
    [ScriptedImporter(1, "genasm")]
    public class GenasmImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            TextAsset desc = new TextAsset("Generated file by Assembly Generator. Please do not remove this file.");
            Texture2D icon = new Texture2D(1, 1);
            try
            {
                string filePath = Path.Combine(Application.dataPath, "Zappy", "Assembly Generator", "Icons", "Icon.png");
                byte[] bytes = File.ReadAllBytes(filePath);
                icon.LoadImage(bytes);
            }
            catch { }

            ctx.AddObjectToAsset("Assembly Info", desc, icon);
        }
    }
}