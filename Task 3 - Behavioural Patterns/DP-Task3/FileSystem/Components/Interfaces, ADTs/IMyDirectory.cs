namespace DP_Task3.FileSystem.Components.Interfaces__ADTs
{
    public interface IMyDirectory : IMyFile
    {
        void AddFile(IMyFile file);

        bool RemoveFile(IMyFile file);

        IMyFile? RemoveFile(string fileName);

        bool ContainsFile(string fileName);
        bool ContainsFile(IMyFile file);
        IReadOnlyCollection<IMyFile> GetFiles();
    }
}
