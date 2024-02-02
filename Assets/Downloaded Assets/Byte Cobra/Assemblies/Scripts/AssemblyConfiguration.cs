using System.Collections.Generic;

namespace ByteCobra.Assemblies
{
    public class AssemblyConfiguration
    {
        public string Path { get; }
        public string RootNamespace { get; }
        public HashSet<Platform> Platforms { get; }
    }
}