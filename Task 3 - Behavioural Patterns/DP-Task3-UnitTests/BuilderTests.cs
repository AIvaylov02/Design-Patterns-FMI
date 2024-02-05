using DP_Task3.FileSystem.Builders;
using DP_Task3.FileSystem.Components;
using DP_Task3.FileSystem.Components.Interfaces__ADTs;
using DP_Task3.Hashers;

namespace DP_Task3_UnitTests
{
    [TestFixture]
    public class SimpleBuilderTests
    {
        string BASE_PATH = HasherHelperFunctions.GetFullPathFromCurrentPoint();
        // build a tree by specifying a non-directory file -> it should just return the file
        [Test]
        public void Test_SimpleBuilder_NonDirectoryFile()
        {
            const string PATH_SUFFIX = "normalPlainTree/empty.txt";
            string path = BASE_PATH + PATH_SUFFIX;
            SimpleFSBuilder fsBuilder = new SimpleFSBuilder(path);
            Assert.That(fsBuilder.FollowLinks, Is.False);
            IMyFile fsRoot = fsBuilder.BuildFileSystem(); // this contains just a plain text file, but it will be wrapped inside a folder
            Assert.That(fsRoot, Is.Not.Null); // keep in mind that if a link is given as root, it can be unpacked this way as the file system gets a fictive new root
            Assert.That(fsRoot, Is.AssignableTo<MyDirectory>());
            Assert.That(((MyDirectory)fsRoot).ContainsFile("empty.txt"), Is.True);
            Assert.That(((MyDirectory)fsRoot).GetFiles().Count, Is.EqualTo(1));
            Assert.That(fsRoot.CalculateSize(), Is.EqualTo(0));
        }

        // build a tree from an empty directory -> emptyDir folder
        [Test]
        public void Test_SimpleBuilder_EmptyDirectory()
        {
            const string PATH_SUFFIX = "emptyDir";
            string path = BASE_PATH + PATH_SUFFIX;
            SimpleFSBuilder fsBuilder = new SimpleFSBuilder(path);
            Assert.That(fsBuilder.FollowLinks, Is.False);
            IMyFile fsRoot = fsBuilder.BuildFileSystem(); // this contains just a plain text file, but it will be wrapped inside a folder
            Assert.That(fsRoot, Is.Not.Null); // keep in mind that if a link is given as root, it can be unpacked this way as the file system gets a fictive new root
            Assert.That(fsRoot, Is.AssignableTo<MyDirectory>());
            Assert.That(((MyDirectory)fsRoot).ContainsFile("empty.txt"), Is.False);
            Assert.That(((MyDirectory)fsRoot).GetFiles().Count, Is.EqualTo(0));
            Assert.That(fsRoot.CalculateSize(), Is.EqualTo(0));
        }
        // build a tree without link objects -> normal tree
        [Test]
        public void Test_SimpleBuilder_TreeWithoutLinks()
        {
            const string PATH_SUFFIX = "normalPlainTree";
            string path = BASE_PATH + PATH_SUFFIX;
            SimpleFSBuilder fsBuilder = new SimpleFSBuilder(path);
            Assert.That(fsBuilder.FollowLinks, Is.False);
            IMyFile fsRoot = fsBuilder.BuildFileSystem(); // this contains just a plain text file, but it will be wrapped inside a folder
            Assert.That(fsRoot, Is.Not.Null); // keep in mind that if a link is given as root, it can be unpacked this way as the file system gets a fictive new root
            Assert.That(fsRoot, Is.AssignableTo<MyDirectory>());
            Assert.That(fsRoot.CalculateSize(), Is.EqualTo(32));

            MyDirectory convertedRoot = (MyDirectory)fsRoot;
            Assert.That(convertedRoot.GetFiles().Count, Is.EqualTo(3));
            Assert.That(convertedRoot.ContainsFile("empty.txt"), Is.True);
            Assert.That(convertedRoot.ContainsFile("Hello world!.txt"), Is.True);
            Assert.That(convertedRoot.ContainsFile("abcOnly"), Is.True);

            // check the nested files are present
            MyDirectory? childDir = convertedRoot.GetFile("abcOnly") as MyDirectory;
            Assert.That(childDir, Is.Not.Null);
            Assert.That(childDir.GetFiles().Count, Is.EqualTo(2));
            Assert.That(childDir.ContainsFile("abc.txt"), Is.True);
            Assert.That(childDir.ContainsFile("abcHello.txt"), Is.True);
            Assert.That(childDir.CalculateSize(), Is.EqualTo(20));
        }
        // build a tree with 3 link objects -> one is linked to root, another is linked to a file and a third is linked to a directory not in the current tree
        [Test]
        public void Test_SimpleBuilder_TreeWithLinks() // check if links are converted back to normal files
        {
            const string PATH_SUFFIX = "TreeWithLinks";
            string path = BASE_PATH + PATH_SUFFIX;
            SimpleFSBuilder fsBuilder = new SimpleFSBuilder(path);
            Assert.That(fsBuilder.FollowLinks, Is.False);
            IMyFile fsRoot = fsBuilder.BuildFileSystem(); // this contains just a plain text file, but it will be wrapped inside a folder
            Assert.That(fsRoot, Is.Not.Null); // keep in mind that if a link is given as root, it can be unpacked this way as the file system gets a fictive new root
            Assert.That(fsRoot, Is.AssignableTo<MyDirectory>());
            Assert.That(fsRoot.CalculateSize(), Is.EqualTo(6000)); // all the link objects will be replaced by their correspondant concrete regular nodes (but saving their size!)
            // size of folder -> 6000 bytes

            MyDirectory convertedRoot = (MyDirectory)fsRoot;
            Assert.That(convertedRoot.GetFiles().Count, Is.EqualTo(3));
            Assert.That(convertedRoot.ContainsFile("SubDir"), Is.True);
            Assert.That(convertedRoot.ContainsFile("empty.txt"), Is.True);
            Assert.That(convertedRoot.ContainsFile("normalPlainTree - Shortcut.lnk"), Is.True);
            Assert.That(convertedRoot.GetFile("normalPlainTree - Shortcut.lnk"), Is.AssignableTo<MyConcreteFile>()); // the lnk obj which was pointing to a different folder is just a normal item now

            // check the nested files are present
            MyDirectory? childDir = convertedRoot.GetFile("SubDir") as MyDirectory;
            Assert.That(childDir, Is.Not.Null);
            Assert.That(childDir.GetFiles().Count, Is.EqualTo(5));
            Assert.That(childDir.ContainsFile("abc.txt"), Is.True);
            Assert.That(childDir.ContainsFile("abcHello.txt"), Is.True);
            Assert.That(childDir.ContainsFile("Hello world!.txt"), Is.True);
            Assert.That(convertedRoot.GetFile("empty.txt - Shortcut.lnk"), Is.AssignableTo<MyConcreteFile>()); // the lnk obj is just a normal file
            Assert.That(convertedRoot.GetFile("TreeWithLinks - Shortcut.lnk"), Is.AssignableTo<MyConcreteFile>()); // the lnk obj is just a normal file
            Assert.That(childDir.CalculateSize(), Is.EqualTo(4111)); // the original file size of the folder with the links
        }
    }



    [TestFixture]
    public class AdvancedBuilderTests // traverse the links, so we may end up with graph attempts (stick to tree structure)
    {
        string BASE_PATH = HasherHelperFunctions.GetFullPathFromCurrentPoint();
        // build a tree by specifying a non-directory file -> it should just return the file
        [Test]
        public void Test_AdvancedFSBuilder_NonDirectoryFile()
        {
            const string PATH_SUFFIX = "normalPlainTree/empty.txt";
            string path = BASE_PATH + PATH_SUFFIX;
            AdvancedFSBuilder fsBuilder = new AdvancedFSBuilder(path);
            Assert.That(fsBuilder.FollowLinks, Is.True);
            IMyFile fsRoot = fsBuilder.BuildFileSystem(); // this contains just a plain text file, but it will be wrapped inside a folder
            Assert.That(fsRoot, Is.Not.Null); // keep in mind that if a link is given as root, it can be unpacked this way as the file system gets a fictive new root
            Assert.That(fsRoot, Is.AssignableTo<MyDirectory>());
            Assert.That(((MyDirectory)fsRoot).ContainsFile("empty.txt"), Is.True);
            Assert.That(((MyDirectory)fsRoot).GetFiles().Count, Is.EqualTo(1));
            Assert.That(fsRoot.CalculateSize(), Is.EqualTo(0));
        }

        // build a tree from an empty directory -> emptyDir folder
        [Test]
        public void Test_AdvancedFSBuilder_EmptyDirectory()
        {
            const string PATH_SUFFIX = "emptyDir";
            string path = BASE_PATH + PATH_SUFFIX;
            AdvancedFSBuilder fsBuilder = new AdvancedFSBuilder(path);
            Assert.That(fsBuilder.FollowLinks, Is.True);
            IMyFile fsRoot = fsBuilder.BuildFileSystem(); // this contains just a plain text file, but it will be wrapped inside a folder
            Assert.That(fsRoot, Is.Not.Null); // keep in mind that if a link is given as root, it can be unpacked this way as the file system gets a fictive new root
            Assert.That(fsRoot, Is.AssignableTo<MyDirectory>());
            Assert.That(((MyDirectory)fsRoot).ContainsFile("empty.txt"), Is.False);
            Assert.That(((MyDirectory)fsRoot).GetFiles().Count, Is.EqualTo(0));
            Assert.That(fsRoot.CalculateSize(), Is.EqualTo(0));
        }
        // build a tree without link objects -> normal tree
        [Test]
        public void Test_AdvancedFSBuilder_TreeWithoutLinks()
        {
            const string PATH_SUFFIX = "normalPlainTree";
            string path = BASE_PATH + PATH_SUFFIX;
            AdvancedFSBuilder fsBuilder = new AdvancedFSBuilder(path);
            Assert.That(fsBuilder.FollowLinks, Is.True);
            IMyFile fsRoot = fsBuilder.BuildFileSystem(); // this contains just a plain text file, but it will be wrapped inside a folder
            Assert.That(fsRoot, Is.Not.Null); // keep in mind that if a link is given as root, it can be unpacked this way as the file system gets a fictive new root
            Assert.That(fsRoot, Is.AssignableTo<MyDirectory>());
            Assert.That(fsRoot.CalculateSize(), Is.EqualTo(32));

            MyDirectory convertedRoot = (MyDirectory)fsRoot;
            Assert.That(convertedRoot.GetFiles().Count, Is.EqualTo(3));
            Assert.That(convertedRoot.ContainsFile("empty.txt"), Is.True);
            Assert.That(convertedRoot.ContainsFile("Hello world!.txt"), Is.True);
            Assert.That(convertedRoot.ContainsFile("abcOnly"), Is.True);

            // check the nested files are present
            MyDirectory? childDir = convertedRoot.GetFile("abcOnly") as MyDirectory;
            Assert.That(childDir, Is.Not.Null);
            Assert.That(childDir.GetFiles().Count, Is.EqualTo(2));
            Assert.That(childDir.ContainsFile("abc.txt"), Is.True);
            Assert.That(childDir.ContainsFile("abcHello.txt"), Is.True);
            Assert.That(childDir.CalculateSize(), Is.EqualTo(20));
        }
        // build a tree with 3 link objects -> one is linked to root, another is linked to a file and a third is linked to a directory not in the current tree
        [Test]
        public void Test_AdvancedFSBuilder_TreeWithLinks() // check if links are replaced with directories or normal files
        {
            const string PATH_SUFFIX = "TreeWithLinks";
            string path = BASE_PATH + PATH_SUFFIX;
            AdvancedFSBuilder fsBuilder = new AdvancedFSBuilder(path);
            Assert.That(fsBuilder.FollowLinks, Is.True);
            IMyFile fsRoot = fsBuilder.BuildFileSystem(); // this contains just a plain text file, but it will be wrapped inside a folder
            Assert.That(fsRoot, Is.Not.Null); // keep in mind that if a link is given as root, it can be unpacked this way as the file system gets a fictive new root
            Assert.That(fsRoot, Is.AssignableTo<MyDirectory>());
            Assert.That(fsRoot.CalculateSize(), Is.EqualTo(64)); // all the link objects will be replaced by their correspondant concrete regular nodes (but saving their size!)
            // size of folder normal items(32 byte) + size of other nodes, which have been added to the tree by following links(32)

            MyDirectory convertedRoot = (MyDirectory)fsRoot;
            Assert.That(convertedRoot.GetFiles().Count, Is.EqualTo(3));
            Assert.That(convertedRoot.ContainsFile("SubDir"), Is.True);
            Assert.That(convertedRoot.ContainsFile("empty.txt"), Is.True);
            Assert.That(convertedRoot.ContainsFile("normalPlainTree - Shortcut.lnk"), Is.False); // the lnk object has been removed
            Assert.That(convertedRoot.ContainsFile("normalPlainTree"), Is.True); // it was replaced by another subtree

            MyDirectory? subTree = convertedRoot.GetFile("normalPlainTree") as MyDirectory;
            Assert.That(subTree, Is.Not.Null);
            Assert.That(subTree.CalculateSize(), Is.EqualTo(32));
            Assert.That(subTree.ContainsFile("empty.txt"), Is.True);
            Assert.That(subTree.ContainsFile("Hello world!.txt"), Is.True);
            Assert.That(subTree.ContainsFileRecursively("abc.txt"), Is.True);
            Assert.That(subTree.ContainsFileRecursively("abcHello.txt"), Is.True);

            subTree = convertedRoot.GetFile("SubDir") as MyDirectory; // go forward with the other subtree
            Assert.That(subTree, Is.Not.Null);
            Assert.That(subTree.CalculateSize(), Is.EqualTo(32));
            Assert.That(subTree.ContainsFile("abc.txt"), Is.True);
            Assert.That(subTree.ContainsFile("Hello world!.txt"), Is.True);
            Assert.That(subTree.ContainsFile("abcHello.txt"), Is.True);

            Assert.That(subTree.ContainsFile("empty.txt - Shortcut.lnk"), Is.False);
            Assert.That(subTree.ContainsFile("empty.txt"), Is.False); // since it is shortcut to a duplicate item, it won't be added to the collection
            Assert.That(subTree.ContainsFile("TreeWithLinks - Shortcut.lnk"), Is.False);
            Assert.That(subTree.ContainsFile("TreeWithLinks"), Is.False); // since it is shortcut to a duplicate item, it won't be added to the collection
        }
    }
}
