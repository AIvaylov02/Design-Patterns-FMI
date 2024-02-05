using System.Text;

namespace DP_Task3.Hashers
{
    public static class HasherHelperFunctions
    {
        public static string GetFullPathToTestingFileForUnitTests(string relativePathInSubfolder)
        {
            return @"../../../Testing-file-system/allFiles/" + relativePathInSubfolder;
        }

        public static string GetFullPathFromCurrentPoint() // this function will traverse from the current path to the /Testing-file-system folder
        {
            const string SEARCHED_DIR = "DP-Task3";
            string[] currentDirPathSplit = Directory.GetCurrentDirectory().Split("\\"); // since the list is unsorted, the best bet we have is linear search
            int numberOfBacks = 0;
            for (int i = currentDirPathSplit.Length - 1; i >= 0; i--)
            {
                if (currentDirPathSplit[i] != SEARCHED_DIR)
                {
                    numberOfBacks++;
                }
                else // the current one is DP-Task3, check if there is still the duplicate DP-Task3 string availabe (happens if the assembly takes part in the Program.cs file)
                {
                    if (currentDirPathSplit[i - 1] == SEARCHED_DIR)
                    {
                        numberOfBacks++;
                    }
                    break;
                }
            }
            StringBuilder sb = new StringBuilder();
            const string BACKSPACE = @"../";
            while (numberOfBacks > 0)
            {
                sb.Append(BACKSPACE);
                numberOfBacks--;
            }
            sb.Append(@"DP-Task3-UnitTests/Testing-file-system/");
            return sb.ToString();
        }

        public static Stream GenerateStream(string sampleString, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            byte[] byteArray = encoding.GetBytes(sampleString);
            MemoryStream memoryStream = new MemoryStream(byteArray);
            return memoryStream;
        }

        // Function to extract link from .lnk file based on filePath. I don't own anything of the function code below, the writer is a MAGICIAN (I passed 3+ hours searching for a way to get the link from the OS)
        public static string ExtractLinkTarget(string filepath) // Source "https://www.appsloveworld.com/csharp/100/309/reading-the-target-of-a-lnk-file-in-c-net-core"
        {
            using (var br = new BinaryReader(System.IO.File.OpenRead(filepath)))
            {
                // skip the first 20 bytes (headersize and linkclsid)
                br.ReadBytes(0x14);
                // read the linkflags structure (4 bytes)
                uint lflags = br.ReadUInt32();
                // if the haslinktargetidlist bit is set then skip the stored idlist 
                // structure and header
                if ((lflags & 0x01) == 1)
                {
                    br.ReadBytes(0x34);
                    var skip = br.ReadUInt16(); // this counts of how far we need to skip ahead
                    br.ReadBytes(skip);
                }
                // get the number of bytes the path contains
                var length = br.ReadUInt32();
                // skip 12 bytes (linkinfoheadersize, linkinfoflgas, and volumeidoffset)
                br.ReadBytes(0x0c);
                // find the location of the localbasepath position
                var lbpos = br.ReadUInt32();
                // skip to the path position 
                // (subtract the length of the read (4 bytes), the length of the skip (12 bytes), and
                // the length of the lbpos read (4 bytes) from the lbpos)
                br.ReadBytes((int)lbpos - 0x14);
                var size = length - lbpos - 0x02;
                var bytepath = br.ReadBytes((int)size);
                var path = Encoding.UTF8.GetString(bytepath, 0, bytepath.Length);
                return path.Replace(@"\", @"/");
            }
        }

        public static string ConvertPathToAbsolute(string relativePath)
        {
            if (relativePath[0] != '/' || relativePath[0] != '\\')
            {
                return Path.GetFullPath(relativePath).Replace("\\", "/");
            }
            return relativePath.Replace("\\", "/");
        }
    }
}
