using System.Text;

namespace DP_Task3.Hashers
{
    public static class HasherHelperFunctions
    {
        public static string GetFullPathToFile(string relativePathInSubfolder)
        {
            return @"../../../Testing-file-system/" + relativePathInSubfolder;
        }

        public static string GetFullPathToFileFromTestingProject(string relativePathInSubfolder)
        {
            return @"../../../../DP-Task3/Testing-file-system/" + relativePathInSubfolder;
        }

        public static Stream GenerateStream(string sampleString, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            byte[] byteArray = encoding.GetBytes(sampleString);
            MemoryStream memoryStream = new MemoryStream(byteArray);
            return memoryStream;
        }
    }
}
