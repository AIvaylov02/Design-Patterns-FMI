using DP_Task3.FileSystem.Components.Interfaces__ADTs;

namespace DP_Task3.FileSystem.Builders
{
    public interface IFSBuilder
    {
        IMyFile BuildFileSystem();

        bool FollowLinks{ get; }
    }
}
