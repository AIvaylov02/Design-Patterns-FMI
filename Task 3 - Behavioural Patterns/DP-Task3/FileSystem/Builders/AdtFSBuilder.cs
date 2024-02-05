using DP_Task3.FileSystem.Components;
using DP_Task3.FileSystem.Components.Interfaces__ADTs;
using DP_Task3.Hashers;

namespace DP_Task3.FileSystem.Builders
{
    public abstract class AdtFSBuilder : IFSBuilder
    {
        string rootPath;
        bool followLinks;

        public AdtFSBuilder(string rootPath, bool followLinks)
        {
            this.rootPath = HasherHelperFunctions.ConvertPathToAbsolute(rootPath);
            this.followLinks = followLinks;
        }
        public AdtFSBuilder(string rootPath) : this(rootPath, false) { }

        // since somebody could give us a defective root, we need to take only the path of it
        public AdtFSBuilder(IMyDirectory root, bool followLinks) : this(root.FilePath, followLinks) { }

        public AdtFSBuilder(IMyDirectory root) : this(root, false) { }

        public virtual IMyFile BuildFileSystem() // build a FileSystem by designating the wanted root
        {
            return BuildFileSystem(rootPath);
        }

        private IMyFile BuildFileSystem(string rootName) // build a file system by specifying a different root (will be used to make directory tree recursively)
        {
            
            if (!Directory.Exists(rootName)) // the file is not a valid directory, it is either a simple file or a link
            {
                IMyFile temporaryRoot = CreateSimpleFile(rootName);
                // the root is of other type, so in order for the process linking to work, we have to return a fictive dir to the algorithm
                string[] parts = temporaryRoot.FilePath.Split(@"/").SkipLast(1).ToArray();
                string startPath = String.Join("//", parts);
                MyDirectory root = new MyDirectory(startPath);
                root.AddFile(temporaryRoot); // add the false root(lnk file or normal file to the traversal of the new root dir)
                return root;
            }
            else
            {
                MyDirectory root = new MyDirectory(rootName); // create the root and load the dir of the current file
                                                              // get all element(files and dirs) names, build each of them and append them to the directory
                string[] children = Directory.GetFiles(rootName).Concat(Directory.GetDirectories(rootName))
                    .Select(x => x.Replace(@"\", @"/")).ToArray(); // normalize the path by placing the "right" slash / instead of \
                foreach (string child in children)
                {
                    root.AddFile(BuildFile(child));
                }
                return root;
            }
        }

        protected IMyFile BuildFile(string path)
        {
            FileAttributes metaInfo = File.GetAttributes(path);
            if ((metaInfo & FileAttributes.Directory) == FileAttributes.Directory) // the file is a directory
            {
                return BuildFileSystem(path);
            }
            else // it is a link or regular file
            {
                return CreateSimpleFile(path);
            }
        }

        private IMyFile CreateSimpleFile(string path)
        {
            FileInfo info = new FileInfo(path);
            string fileExtension = info.Extension;
            if (fileExtension == ".lnk")
            {
                string linkTarget = HasherHelperFunctions.ExtractLinkTarget(path);
                return new MyLink(path, linkTarget, (ulong)info.Length);
            }
            else // normal file is to be created
            {
                return new MyConcreteFile(path, (ulong)info.Length);
            }
        }

        private MyLink BuildLinkFile(string path, ulong size, string target)
        {
            return new MyLink(path, target, size);
        }

        public bool FollowLinks { get => followLinks; private set => followLinks = value; }

        protected abstract MyDirectory ProcessLinkFollowing(MyDirectory dir);
    }
}
