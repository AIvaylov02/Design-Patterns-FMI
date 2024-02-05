using DP_Task3.FileSystem.Components;
using DP_Task3.FileSystem.Components.Interfaces__ADTs;

namespace DP_Task3.FileSystem.Builders
{
    public class SimpleFSBuilder : AdtFSBuilder
    {
        public SimpleFSBuilder(string rootPath) : base(rootPath){}

        public SimpleFSBuilder(IMyDirectory root) : base(root){}

        public override IMyFile BuildFileSystem() // build a FileSystem by designating the wanted root
        {
            MyDirectory root = (MyDirectory)base.BuildFileSystem();
            return ProcessLinkFollowing(root);
        }

        protected override MyDirectory ProcessLinkFollowing(MyDirectory dir) // each Link Node you find needs to be replaced with a standart file node
        {
            IMyFile[] currentFiles = dir.GetFiles().ToArray();
            foreach (var child in currentFiles) // tree traversal - DFS
            {
                if (child is MyLink) // if the child is link, then replace it with a concrete file equiviallent
                {
                    dir.RemoveFile(child);
                    MyConcreteFile processedChild = new MyConcreteFile(child.FilePath, child.CalculateSize());
                    dir.AddFile(processedChild);
                }
                else if (child is MyDirectory)
                {
                    MyDirectory processedChild = ProcessLinkFollowing((MyDirectory)child);
                    dir.RemoveFile(child);
                    dir.AddFile(processedChild);
                }
            }
            return dir;
        }
    }
}
