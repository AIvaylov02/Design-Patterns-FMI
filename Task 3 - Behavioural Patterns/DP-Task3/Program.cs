using DP_Task3.FileSystem.Builders;
using DP_Task3.FileSystem.Components.Interfaces__ADTs;
using DP_Task3.Hashers;

namespace DP_Task3
{
    public class Program
    {
        static void Main(string[] args)
        {
            string BASE_PATH = HasherHelperFunctions.GetFullPathFromCurrentPoint();
            const string PATH_SUFFIX = "TreeWithLinks";
            string path = BASE_PATH + PATH_SUFFIX;
            AdvancedFSBuilder fsBuilder = new AdvancedFSBuilder(path);
            IMyFile fsRoot = fsBuilder.BuildFileSystem(); // this contains just a plain text file, but it will be wrapped inside a folder
            Console.WriteLine(fsRoot.CalculateSize());

        }

        
    }
}
