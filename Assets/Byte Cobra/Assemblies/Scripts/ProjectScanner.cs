namespace ByteCobra.Assemblies
{
    public class ProjectScanner
    {
        public IScanner DirectoryScanner { get; private set; }

        public ProjectScanner(IScanner directoryScanner)
        {
            DirectoryScanner = directoryScanner;
        }
    }
}