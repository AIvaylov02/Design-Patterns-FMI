using DP_Task3.Hashers;
using DP_Task3.Interfaces;

namespace DP_Task3_UnitTests
{
    [TestFixture]
    public class MD5CalculatorTests
    {
        IChecksumCalculator checksumCalculator;
        [SetUp]
        public void Setup()
        {
            checksumCalculator = new MD5Calculator();
        }

        [Test]
        public void Test_MD5_Stream()
        {
            // abc
            string input = "abc";
            using (Stream stream = HasherHelperFunctions.GenerateStream(input))
            {
                string hashResult = checksumCalculator.Calculate(stream);
                const string EXPECTED_RESULT = "900150983cd24fb0d6963f7d28e17f72";
                Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
            }

            // empty
            input = "";
            using (Stream stream = HasherHelperFunctions.GenerateStream(input))
            {
                string hashResult = checksumCalculator.Calculate(stream);
                const string EXPECTED_RESULT = "d41d8cd98f00b204e9800998ecf8427e";
                Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
            }

            // Hello world!
            input = "Hello world!";
            using (Stream stream = HasherHelperFunctions.GenerateStream(input))
            {
                string hashResult = checksumCalculator.Calculate(stream);
                const string EXPECTED_RESULT = "86fb269d190d2c85f6e0468ceca42a20";
                Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
            }

            // combine the two -> abc\nHello world!
            input = "abc\n" + input;
            using (Stream stream = HasherHelperFunctions.GenerateStream(input))
            {
                string hashResult = checksumCalculator.Calculate(stream);
                const string EXPECTED_RESULT = "bfbfc64a20dcc099dd8f446a5e62b8f5";
                Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
            }
        }

        [Test]
        public void Test_MD5_File_EMPTY()
        {
            string fileName = "empty.txt";
            string path = HasherHelperFunctions.GetFullPathToTestingFileForUnitTests(fileName);
            string hashResult = checksumCalculator.Calculate(path);
            const string EXPECTED_RESULT = "d41d8cd98f00b204e9800998ecf8427e";
            Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_MD5_File_abc()
        {
            // file containing abc
            string fileName = "abc.txt";
            string path = HasherHelperFunctions.GetFullPathToTestingFileForUnitTests(fileName);
            string hashResult = checksumCalculator.Calculate(path);
            const string EXPECTED_RESULT = "900150983cd24fb0d6963f7d28e17f72";
            Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_MD5_File_HelloWorld()
        {
            // file containing Hello world!
            string fileName = "Hello world!.txt";
            string path = HasherHelperFunctions.GetFullPathToTestingFileForUnitTests(fileName);
            string hashResult = checksumCalculator.Calculate(path);
            const string EXPECTED_RESULT = "86fb269d190d2c85f6e0468ceca42a20";
            Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_MD5_File_abcHello()
        {
            // file containing abc, new line and Hello world! (Note the hash is different from the plain "abc\n"+"Hello world!" (which https://emn178.github.io/online-tools/md5.html) showed in its converter
            string fileName = "abcHello.txt";
            string path = HasherHelperFunctions.GetFullPathToTestingFileForUnitTests(fileName);
            string hashResult = checksumCalculator.Calculate(path);
            const string EXPECTED_RESULT = "bf41f7ff9743df88a7d0e2f192c50c3f";
            Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
        }
    }



    [TestFixture]
    public class SHA1CalculatorTests
    {
        IChecksumCalculator checksumCalculator;
        [SetUp]
        public void Setup()
        {
            checksumCalculator = new SHA1Calculator();
        }

        [Test]
        public void Test_SHA1_Stream()
        {
            // abc
            string input = "abc";
            using (Stream stream = HasherHelperFunctions.GenerateStream(input))
            {
                string hashResult = checksumCalculator.Calculate(stream);
                const string EXPECTED_RESULT = "a9993e364706816aba3e25717850c26c9cd0d89d";
                Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
            }

            // empty
            input = "";
            using (Stream stream = HasherHelperFunctions.GenerateStream(input))
            {
                string hashResult = checksumCalculator.Calculate(stream);
                const string EXPECTED_RESULT = "da39a3ee5e6b4b0d3255bfef95601890afd80709";
                Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
            }

            // Hello world!
            input = "Hello world!";
            using (Stream stream = HasherHelperFunctions.GenerateStream(input))
            {
                string hashResult = checksumCalculator.Calculate(stream);
                const string EXPECTED_RESULT = "d3486ae9136e7856bc42212385ea797094475802";
                Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
            }

            // combine the two -> abc\nHello world!
            input = "abc\n" + input;
            using (Stream stream = HasherHelperFunctions.GenerateStream(input))
            {
                string hashResult = checksumCalculator.Calculate(stream);
                const string EXPECTED_RESULT = "40f646491ffdc265e6626da6d39e793e47655f9a";
                Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
            }
        }

        [Test]
        public void Test_SHA1_File_EMPTY()
        {
            string fileName = "empty.txt";
            string path = HasherHelperFunctions.GetFullPathToTestingFileForUnitTests(fileName);
            string hashResult = checksumCalculator.Calculate(path);
            const string EXPECTED_RESULT = "da39a3ee5e6b4b0d3255bfef95601890afd80709";
            Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_SHA1_File_abc()
        {
            // file containing abc
            string fileName = "abc.txt";
            string path = HasherHelperFunctions.GetFullPathToTestingFileForUnitTests(fileName);
            string hashResult = checksumCalculator.Calculate(path);
            const string EXPECTED_RESULT = "a9993e364706816aba3e25717850c26c9cd0d89d";
            Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_SHA1_File_HelloWorld()
        {
            // file containing Hello world!
            string fileName = "Hello world!.txt";
            string path = HasherHelperFunctions.GetFullPathToTestingFileForUnitTests(fileName);
            string hashResult = checksumCalculator.Calculate(path);
            const string EXPECTED_RESULT = "d3486ae9136e7856bc42212385ea797094475802";
            Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_SHA1_File_abcHello()
        {
            // file containing abc, new line and Hello world! (Note the hash is different from the plain "abc\n"+"Hello world!" (which https://emn178.github.io/online-tools/md5.html) showed in its converter
            string fileName = "abcHello.txt";
            string path = HasherHelperFunctions.GetFullPathToTestingFileForUnitTests(fileName);
            string hashResult = checksumCalculator.Calculate(path);
            const string EXPECTED_RESULT = "6fae8bb42f0ff3f32090ec44646c83b922724af7";
            Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
        }
    }




    [TestFixture]
    public class SHA512CalculatorTests
    {
        IChecksumCalculator checksumCalculator;
        [SetUp]
        public void Setup()
        {
            checksumCalculator = new SHA512Calculator();
        }

        [Test]
        public void Test_SHA1_Stream()
        {
            // abc
            string input = "abc";
            using (Stream stream = HasherHelperFunctions.GenerateStream(input))
            {
                string hashResult = checksumCalculator.Calculate(stream);
                const string EXPECTED_RESULT = "ddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f";
                Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
            }

            // empty
            input = "";
            using (Stream stream = HasherHelperFunctions.GenerateStream(input))
            {
                string hashResult = checksumCalculator.Calculate(stream);
                const string EXPECTED_RESULT = "cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e";
                Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
            }

            // Hello world!
            input = "Hello world!";
            using (Stream stream = HasherHelperFunctions.GenerateStream(input))
            {
                string hashResult = checksumCalculator.Calculate(stream);
                const string EXPECTED_RESULT = "f6cde2a0f819314cdde55fc227d8d7dae3d28cc556222a0a8ad66d91ccad4aad6094f517a2182360c9aacf6a3dc323162cb6fd8cdffedb0fe038f55e85ffb5b6";
                Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
            }

            // combine the two -> abc\nHello world!
            input = "abc\n" + input;
            using (Stream stream = HasherHelperFunctions.GenerateStream(input))
            {
                string hashResult = checksumCalculator.Calculate(stream);
                const string EXPECTED_RESULT = "90ea5db37f31267bf0950ff218e05ec2768836a6cd20e073a29b827076fefca22b061af7c8dc6bda6953e1170f840cd4b7806112f453c39805e2c733a46fa9cc";
                Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
            }
        }

        [Test]
        public void Test_SHA1_File_EMPTY()
        {
            string fileName = "empty.txt";
            string path = HasherHelperFunctions.GetFullPathToTestingFileForUnitTests(fileName);
            string hashResult = checksumCalculator.Calculate(path);
            const string EXPECTED_RESULT = "cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e";
            Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_SHA1_File_abc()
        {
            // file containing abc
            string fileName = "abc.txt";
            string path = HasherHelperFunctions.GetFullPathToTestingFileForUnitTests(fileName);
            string hashResult = checksumCalculator.Calculate(path);
            const string EXPECTED_RESULT = "ddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f";
            Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_SHA1_File_HelloWorld()
        {
            // file containing Hello world!
            string fileName = "Hello world!.txt";
            string path = HasherHelperFunctions.GetFullPathToTestingFileForUnitTests(fileName);
            string hashResult = checksumCalculator.Calculate(path);
            const string EXPECTED_RESULT = "f6cde2a0f819314cdde55fc227d8d7dae3d28cc556222a0a8ad66d91ccad4aad6094f517a2182360c9aacf6a3dc323162cb6fd8cdffedb0fe038f55e85ffb5b6";
            Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_SHA1_File_abcHello()
        {
            // file containing abc, new line and Hello world! (Note the hash is different from the plain "abc\n"+"Hello world!" (which https://emn178.github.io/online-tools/md5.html) showed in its converter
            string fileName = "abcHello.txt";
            string path = HasherHelperFunctions.GetFullPathToTestingFileForUnitTests(fileName);
            string hashResult = checksumCalculator.Calculate(path);
            const string EXPECTED_RESULT = "8ae05990ea27bb2bf36dd9063c723cca7f79481e9b77c09d743558c528d36893802449745e8d6d63941dbbef14f3e6df785d72b1c32d0630b2bd488abcd007b2";
            Assert.That(hashResult, Is.EqualTo(EXPECTED_RESULT));
        }
    }
}