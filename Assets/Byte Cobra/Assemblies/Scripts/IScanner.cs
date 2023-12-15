using System.Collections.Generic;
using System.IO;

namespace ByteCobra.Assemblies
{
    public interface IScanner
    {
        IEnumerable<string> Scan(DirectoryInfo directory);
    }
}