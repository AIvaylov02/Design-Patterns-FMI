using DP_Task3.FileSystem.Components.Interfaces__ADTs;

namespace DP_Task3.FileSystem.Components
{
    public class MyConcreteFile : MyADTFile
    {
        ulong size; // the size of the actual file (will be given by the builder)
        public MyConcreteFile(string path, ulong size) : base(path)
        {
            this.size = size;
        }
        public override ulong CalculateSize()
        {
            return size;
        }
    }
}
