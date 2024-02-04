using DP_Task3.FileSystem.Components.Interfaces__ADTs;

namespace DP_Task3.FileSystem.Components
{
    public class MyDirectory : MyADTFile, IMyDirectory
    {
        SortedSet<IMyFile> files; // files are usually sorted by name (default set/dict sorting in c# sorts by time of addition)
        public MyDirectory(string path) : base(path)
        {
            files = new SortedSet<IMyFile>(new CustomFileNameComparator());
        }

        public void AddFile(IMyFile file)
        {
            files.Add(file);
        }

        public override ulong CalculateSize()
        {
            ulong size = 0; // Theoretically size could be stored and modified when a new object(file) is added/removed from the folder, but this may lead to false state
            // as a file can be modified, therefore its size will be different from the suurely known state(Of course observer could be implemented in a later stage, but for know, we will stick to naive calculations every time)
            foreach (IMyFile file in files)
            {
                size += file.CalculateSize();
            }
            return size;
        }

        public bool ContainsFile(string fileName)
        {
            HashSet<string> fileNames = files.Select(item =>
            {
                string[] filePathSplit = item.FilePath.Split('/');
                return filePathSplit[filePathSplit.Length - 1];
            }).ToHashSet();
            return fileNames.Contains(fileName);
        }

        public bool ContainsFile(IMyFile file)
        {
            return files.Contains(file);
        }

        public IMyFile? RemoveFile(string fileName)
        {
            // get the file obj by specifying fileName
            IMyFile[] matchedFiles = files.Where(item =>
            {
                string[] splitFilePath = item.FilePath.Split("/");
                return splitFilePath[splitFilePath.Length - 1] == fileName;
            }).ToArray();

            IMyFile? searchedFile = null;
            if (matchedFiles.Length != 0) // there is an item with this name
            {
                searchedFile = matchedFiles[0];
                files.Remove(searchedFile);
            }
            return searchedFile;
        }

        public bool RemoveFile(IMyFile file)
        {
            return files.Remove(file);
        }

        public IReadOnlyCollection<IMyFile> GetFiles()
        {
            return files;
        }

        public class CustomFileNameComparator : IComparer<IMyFile> // custom comparer for the file tree (worse alternative would be to use IComparable, as it is buggy with reference types)
        {
            // for testing purposes it is marked as public, it should be private for the oop sense of encapsulation/abstraction
            public int Compare(IMyFile? first, IMyFile? second)
            {
                if (first is null && second is null)
                    return 0;
                if (first is not null && second is null)
                    return 1;
                if (first is null && second is not null)
                    return -1;
                return first.FilePath.CompareTo(second.FilePath);
            }
        }
    }
}
