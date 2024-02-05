using DP_Task3.FileSystem.Components;
using DP_Task3.FileSystem.Components.Interfaces__ADTs;

namespace DP_Task3.FileSystem.Builders
{
    public class AdvancedFSBuilder : AdtFSBuilder
    {
        HashSet<string> builtFiles; // will store the already visited/built files as cycles are acceptable in a graph

        public AdvancedFSBuilder(string rootPath) : base(rootPath, true) 
        {
            builtFiles = new HashSet<string>();
        }

        public AdvancedFSBuilder(IMyDirectory root) : base(root, true) 
        {
            builtFiles = new HashSet<string>();
        }

        public override IMyFile BuildFileSystem() // build a FileSystem by designating the wanted root
        {
            MyDirectory root = (MyDirectory)base.BuildFileSystem();
            ResetVisited();
            builtFiles.Add(root.FilePath);
            return ProcessLinkFollowing(root);
        }

        protected override MyDirectory ProcessLinkFollowing(MyDirectory dir)
        {
            IMyFile[] currentFiles = dir.GetFiles().ToArray();
            foreach (var child in currentFiles) // graph traversal
            {
                if (builtFiles.Contains(child.FilePath)) // the file has already been added(build), => skip it
                {
                    dir.RemoveFile(child); // remove it so that no duplicates are left
                    continue;
                }

                builtFiles.Add(child.FilePath);

                if (child is MyLink) // if the child is link, then replace it with a concrete file equiviallent
                {
                    dir.RemoveFile(child);
                    // if link points to a folder, create a folder; if it points to a file create just a simple file
                    MyLink link = (MyLink)child;
                    IMyFile pointee = BuildFile(link.PointedObjectPath);

                    ulong numberOfConcreteAdditions = 0;
                    while (!builtFiles.Contains(pointee.FilePath))
                    {
                        builtFiles.Add(pointee.FilePath);
                        numberOfConcreteAdditions++;
                        if (pointee is MyLink)
                        {
                            link = (MyLink)pointee;
                            pointee = BuildFile(link.PointedObjectPath);
                        }
                    }

                    if (pointee is MyDirectory)
                    {
                        if (numberOfConcreteAdditions != 0)
                        {
                            pointee = ProcessLinkFollowing((MyDirectory)pointee);
                            dir.AddFile(pointee);
                        }
                    }
                    else // it is a concrete file
                    {
                        if (!builtFiles.Contains(pointee.FilePath)) // if it yet to have been added
                            dir.AddFile(pointee); // append the newly create concrete file
                    }
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

        private void ResetVisited()
        {
            if (builtFiles.Count != 0)
            {
                builtFiles.Clear();
            }
        }
    }
}
