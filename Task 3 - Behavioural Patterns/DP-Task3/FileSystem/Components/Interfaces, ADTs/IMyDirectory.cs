namespace DP_Task3.FileSystem.Components.Interfaces__ADTs
{
    public interface IMyDirectory : IMyFile
    {
        void AddFile(IMyFile file);

        bool RemoveFile(IMyFile file);

        IMyFile? RemoveFile(string fileName);

        bool ContainsFile(string fileName);
        bool ContainsFile(IMyFile file);

        bool ContainsFileRecursively(string fileName);

        // Get all direct children of the directory
        IReadOnlyCollection<IMyFile> GetFiles();

        // Get file object by specifying fileName. Will return Null if file is not found within directory.
        IMyFile? GetFile(string fileName);
    }
}
