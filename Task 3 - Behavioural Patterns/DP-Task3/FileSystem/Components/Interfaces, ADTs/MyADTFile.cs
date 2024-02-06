using DP_Task3.Visitors.Interfaces__ADTs;

namespace DP_Task3.FileSystem.Components.Interfaces__ADTs
{
    public abstract class MyADTFile : IMyFile
    {
        string path;

        public MyADTFile(string path) // as the files will be build by the builder, each file will receive its path
        {
            FilePath = path;
        }

        public abstract ulong CalculateSize();


        public bool Equals(IMyFile? other) // this is needed in order to sucessfully remove or add files from a directory
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return path == other.FilePath;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as IMyFile);
        }

        public string FilePath
        {
            get => path;
            private set
            {
                path = value;
            }
        }

        public virtual string Accept(IVisitor visitor, string? basePath = null)
        {
            return visitor.VisitConcreteFile(this, basePath);
        }

        public string GetRelativePath(string absoluteRoot)
        {
            return Path.GetRelativePath(absoluteRoot, path).Replace(@"\", @"/");
        }
    }
}
