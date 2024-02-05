using DP_Task3.FileSystem.Components;
using DP_Task3.FileSystem.Components.Interfaces__ADTs;

namespace DP_Task3_UnitTests
{
    [TestFixture]
    public class MyConcreteFileTests
    {
        const string path = @"/root/abcCompany/XMLProject/file1.txt";
        const int SIZE = 10;
        IMyFile file;
        [SetUp]
        public void SetUp()
        {
            file = new MyConcreteFile(path, SIZE);
        }

        [Test]
        public void Test_Concrete_File_Initalization()
        {
            Assert.That(file, Is.Not.Null);
            Assert.That(file.FilePath, Is.EqualTo(path));
            Assert.That(file.CalculateSize(), Is.EqualTo(SIZE));
        }

        [Test]
        public void Test_Concrete_File_Equals()
        {
            IMyFile anotherFile = new MyConcreteFile(path, SIZE - 5);
            Assert.That(ReferenceEquals(file, anotherFile), Is.False);
            Assert.That(file, Is.EqualTo(anotherFile));
            anotherFile = new MyDirectory(path);
            Assert.That(ReferenceEquals(file, anotherFile), Is.False);
            Assert.That(file, Is.EqualTo(anotherFile)); // this should be impossible btw, but for the sake of learning purposes, let's suppose a file can be a folder and a regular file simultaniously
            MyConcreteFile thirdFile = new MyConcreteFile(path, SIZE - SIZE);
            Assert.That(ReferenceEquals(file, thirdFile), Is.False);
            Assert.That(file, Is.EqualTo(thirdFile));
        }
    }

    [TestFixture]
    public class MyDirectoryTests
    {
        const string path = @"/root/abcCompany/XMLProject";
        const int SIZE = 10;
        IMyFile file;
        IMyFile anotherFile;

        [SetUp]
        public void SetUp()
        {
            file = new MyDirectory(path);
            anotherFile = new MyConcreteFile(path + @"/text1.txt", SIZE - 5);
        }

        [Test]
        public void Test_Directory_Initalization()
        {
            Assert.That(file, Is.Not.Null);
            Assert.That(file.FilePath, Is.EqualTo(path));
            Assert.That(file.CalculateSize(), Is.EqualTo(0));
            MyDirectory? dir = file as MyDirectory;
            Assert.That(dir, Is.Not.Null);
            Assert.That(dir.GetFiles(), Is.EquivalentTo(new SortedSet<IMyFile>()));
        }

        [Test]
        public void Test_Directory_Equals()
        {
            Assert.That(ReferenceEquals(file, anotherFile), Is.False);
            anotherFile = new MyConcreteFile(path, SIZE - 5);
            Assert.That(file, Is.EqualTo(anotherFile)); // this should be impossible btw, but for the sake of learning purposes, let's suppose a file can be a folder and a regular file simultaniously
            anotherFile = new MyDirectory(path);
            Assert.That(ReferenceEquals(file, anotherFile), Is.False);
            Assert.That(file, Is.EqualTo(anotherFile));
        }

        [Test]
        public void Test_Directory_Add_File()
        {
            MyDirectory? dir = file as MyDirectory;
            Assert.That(dir, Is.Not.Null);

            dir.AddFile(anotherFile);
            Assert.That(dir.ContainsFile(anotherFile), Is.True);
            SortedSet<IMyFile> currentFiles = new SortedSet<IMyFile>(new MyDirectory.CustomFileNameComparator()) { anotherFile };
            Assert.That(dir.GetFiles(), Is.EquivalentTo(currentFiles));

            anotherFile = new MyConcreteFile(path + @"/text2.txt", SIZE + 5);
            dir.AddFile(anotherFile);
            Assert.That(dir.ContainsFile(anotherFile), Is.True);
            currentFiles.Add(anotherFile);
            Assert.That(dir.GetFiles(), Is.EquivalentTo(currentFiles));
        }

        [Test]
        public void Test_Directory_Search_File()
        {
            MyDirectory? dir = file as MyDirectory;
            Assert.That(dir, Is.Not.Null);

            dir.AddFile(anotherFile);
            Assert.That(dir.ContainsFile(anotherFile), Is.True);
            SortedSet<IMyFile> currentFiles = new SortedSet<IMyFile>(new MyDirectory.CustomFileNameComparator()) { anotherFile };
            Assert.That(dir.GetFiles(), Is.EquivalentTo(currentFiles));

            anotherFile = new MyConcreteFile(path + @"/text2.txt", SIZE + 5);
            dir.AddFile(anotherFile);
            Assert.That(dir.ContainsFile(anotherFile), Is.True);
            currentFiles.Add(anotherFile);
            Assert.That(dir.GetFiles(), Is.EquivalentTo(currentFiles));
            Assert.That(dir.ContainsFile("text2.txt"), Is.True);
            Assert.That(dir.ContainsFile("text1.txt"), Is.True);
            Assert.That(dir.ContainsFile("minyor.txt"), Is.False);
        }

        [Test]
        public void Test_Directory_Search_File_Recursively()
        {
            MyDirectory? dir = file as MyDirectory;
            Assert.That(dir, Is.Not.Null);

            dir.AddFile(anotherFile);
            Assert.That(dir.ContainsFileRecursively(anotherFile.FilePath.Split(@"/").TakeLast(1).First()), Is.True); // get only the fileName
            SortedSet<IMyFile> currentFiles = new SortedSet<IMyFile>(new MyDirectory.CustomFileNameComparator()) { anotherFile };
            Assert.That(dir.GetFiles(), Is.EquivalentTo(currentFiles));

            anotherFile = new MyConcreteFile(path + @"/text2.txt", SIZE + 5);
            dir.AddFile(anotherFile);
            Assert.That(dir.ContainsFileRecursively(anotherFile.FilePath.Split(@"/").TakeLast(1).First()), Is.True); // get only the fileName
            currentFiles.Add(anotherFile);
            Assert.That(dir.GetFiles(), Is.EquivalentTo(currentFiles));
            Assert.That(dir.ContainsFileRecursively("text2.txt"), Is.True);
            Assert.That(dir.ContainsFileRecursively("text1.txt"), Is.True);
            Assert.That(dir.ContainsFileRecursively("minyor.txt"), Is.False);

            MyDirectory newDir = new MyDirectory(path + @"/dir2"); // add a subfolder with a file to the main folder
            dir.AddFile(newDir);
            anotherFile = new MyConcreteFile(newDir.FilePath + @"/text3.txt", SIZE);
            newDir.AddFile(anotherFile);
            Assert.That(dir.ContainsFile("text3.txt"), Is.False);
            Assert.That(dir.ContainsFileRecursively("text3.txt"), Is.True);
        }

        [Test]
        public void Test_Directory_GetFile()
        {
            MyDirectory? dir = file as MyDirectory;
            Assert.That(dir, Is.Not.Null);

            dir.AddFile(anotherFile);
            IMyFile? extracted = dir.GetFile(anotherFile.FilePath.Split(@"/").TakeLast(1).First()); // get file obj by specifying name
            Assert.That(extracted, Is.Not.Null);
            Assert.That(extracted, Is.EqualTo(anotherFile));

            anotherFile = new MyConcreteFile(path + @"/text2.txt", SIZE + 5);
            dir.AddFile(anotherFile);
            extracted = dir.GetFile(anotherFile.FilePath.Split(@"/").TakeLast(1).First());
            Assert.That(extracted, Is.Not.Null);
            Assert.That(extracted, Is.EqualTo(anotherFile));

            anotherFile = new MyConcreteFile(path + @"/minyor.txt", SIZE + 5); // create a file but don't add it
            extracted = dir.GetFile(anotherFile.FilePath.Split(@"/").TakeLast(1).First());
            Assert.That(extracted, Is.Null);
            Assert.That(extracted, Is.Not.EqualTo(anotherFile));

            MyDirectory newDir = new MyDirectory(path + @"/dir2"); // add a subfolder with a file to the main folder
            dir.AddFile(newDir); // add the subfolder to the main folder
            anotherFile = new MyConcreteFile(newDir.FilePath + @"/text3.txt", SIZE);
            newDir.AddFile(anotherFile); // add the file to the subfolder
            extracted = dir.GetFile(anotherFile.FilePath.Split(@"/").TakeLast(1).First());
            Assert.That(extracted, Is.Not.Null);
            Assert.That(extracted, Is.EqualTo(anotherFile));
        }

        [Test]
        public void Test_Directory_Remove_File()
        {
            MyDirectory? dir = file as MyDirectory;
            Assert.That(dir, Is.Not.Null);

            dir.AddFile(anotherFile);
            SortedSet<IMyFile> currentFiles = new SortedSet<IMyFile>(new MyDirectory.CustomFileNameComparator()) { anotherFile };
            anotherFile = new MyConcreteFile(path + @"/text2.txt", SIZE + 5);
            currentFiles.Add(anotherFile);
            dir.AddFile(anotherFile);
            Assert.That(dir.ContainsFile("text1.txt"), Is.True);
            Assert.That(dir.ContainsFile("text2.txt"), Is.True);
            Assert.That(dir.GetFiles(), Is.EquivalentTo(currentFiles));

            Assert.That(dir.RemoveFile(anotherFile), Is.True);
            Assert.That(dir.RemoveFile(anotherFile), Is.False); // remove the same file twice
            currentFiles.Remove(anotherFile);
            Assert.That(dir.GetFiles(), Is.EquivalentTo(currentFiles));

            IMyFile? removed = dir.RemoveFile("text1.txt");
            Assert.That(removed, Is.Not.Null);
            Assert.That(removed.FilePath, Is.EqualTo(path + @"/text1.txt"));
            currentFiles.Remove(removed);
            Assert.That(dir.GetFiles(), Is.EquivalentTo(currentFiles)); // empty collections

            removed = dir.RemoveFile("text1.txt"); // try to remove the same file twice
            Assert.That(removed, Is.Null);
            Assert.That(dir.GetFiles(), Is.EquivalentTo(currentFiles)); // empty collections
        }

        [Test]
        public void Test_Directory_Calculate_Sizes()
        {
            MyDirectory? dir = file as MyDirectory;
            Assert.That(dir, Is.Not.Null);
            Assert.That(file.CalculateSize(), Is.EqualTo(0));

            dir.AddFile(anotherFile);
            ulong sum = 0;
            sum += anotherFile.CalculateSize();
            Assert.That(file.CalculateSize(), Is.EqualTo(sum));

            anotherFile = new MyConcreteFile(path + @"/text2.txt", SIZE + 5);
            sum += anotherFile.CalculateSize();
            dir.AddFile(anotherFile);
            Assert.That(file.CalculateSize(), Is.EqualTo(sum));

            dir.RemoveFile(anotherFile);
            sum -= anotherFile.CalculateSize();
            Assert.That(file.CalculateSize(), Is.EqualTo(sum));

            MyDirectory nestedDirectory = new MyDirectory(dir.FilePath + @"/OtherStuff");
            nestedDirectory.AddFile(new MyConcreteFile(nestedDirectory.FilePath + @"/important.txt", 20));
            dir.AddFile(nestedDirectory);
            sum += 20;
            Assert.That(file.CalculateSize(), Is.EqualTo(sum));
        }
    }

    [TestFixture]
    public class MyLinkTests
    {
        const string PATH = @"/root/abcCompany/XMLProject";
        const string LINK_PATH_SUFFIX = @"/my_link.lnk";
        const string DIRECTORY_FILE_PATH_SUFFIX = @"/test-dir";
        const string ANOTHER_FILE_PATH_SUFFIX = DIRECTORY_FILE_PATH_SUFFIX + @"/abc.txt";
        const string SECOND_FILE_PATH_SUFFIX = DIRECTORY_FILE_PATH_SUFFIX + @"/abc2.txt";
        const int SIZE = 10;

        [Test]
        public void Test_Link_Initalization()
        {
            IMyFile baseFile = new MyLink(PATH + LINK_PATH_SUFFIX, PATH + ANOTHER_FILE_PATH_SUFFIX, SIZE);
            Assert.That(baseFile.FilePath, Is.EqualTo(PATH + LINK_PATH_SUFFIX));
            Assert.That(baseFile.CalculateSize(), Is.EqualTo(SIZE));

            MyLink? link = baseFile as MyLink;
            Assert.That(link, Is.Not.Null);
            Assert.That(link.PointedObjectPath, Is.EqualTo(PATH + ANOTHER_FILE_PATH_SUFFIX));

            IMyFile anotherFile = new MyConcreteFile(PATH + SECOND_FILE_PATH_SUFFIX, 30); // try to point to a simple file
            baseFile = new MyLink(PATH + LINK_PATH_SUFFIX, anotherFile, SIZE);
            link = baseFile as MyLink;
            Assert.That(link, Is.Not.Null);
            Assert.That(link.PointedObjectPath, Is.EqualTo(PATH + SECOND_FILE_PATH_SUFFIX));

            anotherFile = new MyDirectory(PATH + DIRECTORY_FILE_PATH_SUFFIX); // try to point to a directory
            baseFile = new MyLink(PATH + LINK_PATH_SUFFIX, anotherFile, SIZE);
            link = baseFile as MyLink;
            Assert.That(link, Is.Not.Null);
            Assert.That(link.PointedObjectPath, Is.EqualTo(PATH + DIRECTORY_FILE_PATH_SUFFIX));
        }

        [Test]
        public void Test_Concrete_File_Equals()
        {
            IMyFile link = new MyLink(PATH + LINK_PATH_SUFFIX, PATH + ANOTHER_FILE_PATH_SUFFIX, SIZE);
            IMyFile anotherFile = new MyConcreteFile(PATH + ANOTHER_FILE_PATH_SUFFIX, SIZE + 5);
            Assert.That(ReferenceEquals(link, anotherFile), Is.False);
            Assert.That(link, Is.Not.EqualTo(anotherFile)); // this shouldn't happen if they have different paths

            anotherFile = new MyDirectory(PATH + DIRECTORY_FILE_PATH_SUFFIX);
            Assert.That(ReferenceEquals(link, anotherFile), Is.False);
            Assert.That(link, Is.Not.EqualTo(anotherFile));

            anotherFile = new MyConcreteFile(PATH + LINK_PATH_SUFFIX, SIZE + 5); // given the same name, a link obj will be equal to a concrete obj/dir obj, as it is an invalid case in the filesystem
            Assert.That(ReferenceEquals(link, anotherFile), Is.False);
            Assert.That(link, Is.EqualTo(anotherFile));
        }
    }
}
