using DP_Task3.FileSystem.Components.Interfaces__ADTs;

namespace DP_Task3.FileSystem.Components
{
    public class MyLink : MyADTFile
    {
        ulong size; // the size of the sym link object (will be given by the builder)
        string pointsTo;
        public MyLink(string path, string pointsTo, ulong size) : base(path)
        {
            this.size = size;
            PointedObjectPath = pointsTo; // set the points to to a living object in the fileSystem
        }

        public MyLink(string path, IMyFile file, ulong size) : base(path)
        {
            this.size = size;
            PointedObjectPath = file.FilePath; // set the points to to a living object in the fileSystem
        }
        public override ulong CalculateSize()
        {
            return size;
        }

        public string PointedObjectPath
        {
            get => pointsTo;
            private set => pointsTo = value;
        }
    }
}
