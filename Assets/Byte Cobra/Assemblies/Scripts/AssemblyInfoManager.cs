using System.IO;

namespace ByteCobra.Assemblies
{
    public class AssemblyInfoManager
    {
        public const string FileName = "AssemblyGenerator.genasm";

        /// <summary>
        /// Creates a new metadata file.
        /// </summary>
        /// <param name="directory"></param>
        public void Create(string directory)
        {
            string fullDirPath = Path.GetFullPath(directory);
            string path = Path.Combine(fullDirPath, FileName);

            if (!File.Exists(path))
                File.WriteAllBytes(path, new byte[0]);
        }

        /// <summary>
        /// Deletes the metadata file in the given directory, if it exists.
        /// </summary>
        /// <param name="directory"></param>
        public void Delete(string directory)
        {
            string fullDirPath = Path.GetFullPath(directory);
            string path = Path.Combine(fullDirPath, FileName);

            if (!File.Exists(path))
                File.Delete(path);
        }

        /// <summary>
        /// Returns true if this directory has a meta data file.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public bool Exists(string directory)
        {
            string fullDirPath = Path.GetFullPath(directory);
            string path = Path.Combine(fullDirPath, FileName);
            return File.Exists(path);
        }
    }
}