using System.IO;
using System.Linq;
using UnityEngine;

namespace ByteCobra.Assemblies
{
    public class AssemblyReplacer
    {
        private const string Postfix = "_old";

        public void Replace(string directory, string extension)
        {
            directory = Path.GetFullPath(directory);
            var file = Directory.GetFiles(directory).FirstOrDefault(x => x.EndsWith(extension));
            if (file != null)
            {
                var newPath = Path.GetFullPath(file + Postfix);
                File.Move(file, newPath);
                Debug.Log("<color=cyan>Renamed: </color><color=white> " + file + " to " + Path.GetFileName(newPath) + "</color>");
            }
        }

        public void Restore(string directory, string extension)
        {
            directory = Path.GetFullPath(directory);
            var file = Directory.GetFiles(directory).FirstOrDefault(x => x.EndsWith(extension + Postfix));

            if (file != null)
            {
                file = Path.GetFullPath(file);
                string newPath = file.Replace("_old", "");
                File.Move(file, newPath);
                Debug.Log("<color=cyan>Renamed: </color><color=white> " + file + " to " + Path.GetFileName(newPath) + "</color>");
            }
        }
    }
}