using DP_Task3.FileSystem.Builders;
using DP_Task3.FileSystem.Components.Interfaces__ADTs;
using DP_Task3.Hashers;
using DP_Task3.Visitors;
using DP_Task3.Visitors.Interfaces__ADTs;

namespace DP_Task3_UnitTests
{
    [TestFixture]
    public class VisitorTests
    {
        string BASE_PATH = HasherHelperFunctions.GetFullPathFromCurrentPoint();
        const string OS_PATH_SUFFIX = "TreeWithLinks";
        string path;
        IMyFile fs;

        [SetUp]
        public void SetUp()
        {
            path = BASE_PATH + OS_PATH_SUFFIX;
            IFSBuilder builder = new AdvancedFSBuilder(path);
            fs = builder.BuildFileSystem();
        }

        [Test]
        public void Test_ReportWriter()
        {
            IVisitor reporter = new ReportWriter();
            string result = fs.Accept(reporter, path);
            const string EXPECTED_RAW_RESULT = "   64   with rel_path: .\r\n   32   with rel_path: ../normalPlainTree\r\n   20   with rel_path: ../normalPlainTree/abcOnly\r\n    3   with rel_path: ../normalPlainTree/abcOnly/abc.txt\r\n   17   with rel_path: ../normalPlainTree/abcOnly/abcHello.txt\r\n\r\n    0   with rel_path: ../normalPlainTree/empty.txt\r\n   12   with rel_path: ../normalPlainTree/Hello world!.txt\r\n\r\n    0   with rel_path: empty.txt\r\n   32   with rel_path: SubDir\r\n    3   with rel_path: SubDir/abc.txt\r\n   17   with rel_path: SubDir/abcHello.txt\r\n   12   with rel_path: SubDir/Hello world!.txt\r\n\r\n";
            Assert.That(result, Is.EqualTo(EXPECTED_RAW_RESULT));
            result = VisitorResultFormatter.FormatResults(result);
            const string EXPECTED_FORMATTED_RESULT = "  File0:    64   with rel_path: .\r\n  File1:    32   with rel_path: ../normalPlainTree\r\n  File2:    20   with rel_path: ../normalPlainTree/abcOnly\r\n  File3:     3   with rel_path: ../normalPlainTree/abcOnly/abc.txt\r\n  File4:    17   with rel_path: ../normalPlainTree/abcOnly/abcHello.txt\r\n  File5:     0   with rel_path: ../normalPlainTree/empty.txt\r\n  File6:    12   with rel_path: ../normalPlainTree/Hello world!.txt\r\n  File7:     0   with rel_path: empty.txt\r\n  File8:    32   with rel_path: SubDir\r\n  File9:     3   with rel_path: SubDir/abc.txt\r\n File10:    17   with rel_path: SubDir/abcHello.txt\r\n File11:    12   with rel_path: SubDir/Hello world!.txt";
            Assert.That(result, Is.EqualTo(EXPECTED_FORMATTED_RESULT));
        }

        [Test]
        public void Test_HashWriter_UnknownType()
        {
            string hashingMethod = "NewWayHash";
            Assert.Throws<ArgumentException>(() => new HashStreamWriter(hashingMethod));
            hashingMethod = "MD5";
            Assert.DoesNotThrow(() => new HashStreamWriter(hashingMethod));
            hashingMethod = "SHA1";
            Assert.DoesNotThrow(() => new HashStreamWriter(hashingMethod));
            hashingMethod = "SHA512";
            Assert.DoesNotThrow(() => new HashStreamWriter(hashingMethod));
        }

        [Test]
        public void Test_HashWriter_MD5_NoFollowing()
        {
            const string HASHING_METHOD = "MD5";
            IVisitor baseHasher = new HashStreamWriter(HASHING_METHOD);
            HashStreamWriter? hasher = baseHasher as HashStreamWriter;
            Assert.That(hasher, Is.Not.Null);
            Assert.That(hasher.Hasher, Is.EqualTo(HASHING_METHOD));
            IFSBuilder builder = new SimpleFSBuilder(path);
            fs = builder.BuildFileSystem();

            string result = fs.Accept(hasher, path);
            const string EXPECTED_RAW_RESULT = "\r\nd41d8cd98f00b204e9800998ecf8427e   with rel_path: empty.txt\r\n84765e531abc3fee71f6de3b8672ffe4   with rel_path: normalPlainTree - Shortcut.lnk\r\n\r\n900150983cd24fb0d6963f7d28e17f72   with rel_path: SubDir/abc.txt\r\nbf41f7ff9743df88a7d0e2f192c50c3f   with rel_path: SubDir/abcHello.txt\r\n7b7d75e70c26358d3a24c243024a3363   with rel_path: SubDir/empty.txt - Shortcut.lnk\r\n86fb269d190d2c85f6e0468ceca42a20   with rel_path: SubDir/Hello world!.txt\r\n39a193a2027c91f572dda7cdd4170756   with rel_path: SubDir/TreeWithLinks - Shortcut.lnk\r\n\r\n";
            Assert.That(result, Is.EqualTo(EXPECTED_RAW_RESULT));
            result = VisitorResultFormatter.FormatResults(result);
            const string EXPECTED_FORMATTED_RESULT = "  File0: d41d8cd98f00b204e9800998ecf8427e   with rel_path: empty.txt\r\n  File1: 84765e531abc3fee71f6de3b8672ffe4   with rel_path: normalPlainTree - Shortcut.lnk\r\n  File2: 900150983cd24fb0d6963f7d28e17f72   with rel_path: SubDir/abc.txt\r\n  File3: bf41f7ff9743df88a7d0e2f192c50c3f   with rel_path: SubDir/abcHello.txt\r\n  File4: 7b7d75e70c26358d3a24c243024a3363   with rel_path: SubDir/empty.txt - Shortcut.lnk\r\n  File5: 86fb269d190d2c85f6e0468ceca42a20   with rel_path: SubDir/Hello world!.txt\r\n  File6: 39a193a2027c91f572dda7cdd4170756   with rel_path: SubDir/TreeWithLinks - Shortcut.lnk";
            Assert.That(result, Is.EqualTo(EXPECTED_FORMATTED_RESULT));
        }

        [Test]
        public void Test_HashWriter_MD5_Following()
        {
            const string HASHING_METHOD = "MD5";
            IVisitor baseHasher = new HashStreamWriter(HASHING_METHOD);
            HashStreamWriter? hasher = baseHasher as HashStreamWriter;
            Assert.That(hasher, Is.Not.Null);
            Assert.That(hasher.Hasher, Is.EqualTo(HASHING_METHOD));

            string result = fs.Accept(hasher, path);
            const string EXPECTED_RAW_RESULT = "\r\n\r\n\r\n900150983cd24fb0d6963f7d28e17f72   with rel_path: ../normalPlainTree/abcOnly/abc.txt\r\nbf41f7ff9743df88a7d0e2f192c50c3f   with rel_path: ../normalPlainTree/abcOnly/abcHello.txt\r\n\r\nd41d8cd98f00b204e9800998ecf8427e   with rel_path: ../normalPlainTree/empty.txt\r\n86fb269d190d2c85f6e0468ceca42a20   with rel_path: ../normalPlainTree/Hello world!.txt\r\n\r\nd41d8cd98f00b204e9800998ecf8427e   with rel_path: empty.txt\r\n\r\n900150983cd24fb0d6963f7d28e17f72   with rel_path: SubDir/abc.txt\r\nbf41f7ff9743df88a7d0e2f192c50c3f   with rel_path: SubDir/abcHello.txt\r\n86fb269d190d2c85f6e0468ceca42a20   with rel_path: SubDir/Hello world!.txt\r\n\r\n";
            Assert.That(result, Is.EqualTo(EXPECTED_RAW_RESULT));
            result = VisitorResultFormatter.FormatResults(result);
            const string EXPECTED_FORMATTED_RESULT = "  File0: 900150983cd24fb0d6963f7d28e17f72   with rel_path: ../normalPlainTree/abcOnly/abc.txt\r\n  File1: bf41f7ff9743df88a7d0e2f192c50c3f   with rel_path: ../normalPlainTree/abcOnly/abcHello.txt\r\n  File2: d41d8cd98f00b204e9800998ecf8427e   with rel_path: ../normalPlainTree/empty.txt\r\n  File3: 86fb269d190d2c85f6e0468ceca42a20   with rel_path: ../normalPlainTree/Hello world!.txt\r\n  File4: d41d8cd98f00b204e9800998ecf8427e   with rel_path: empty.txt\r\n  File5: 900150983cd24fb0d6963f7d28e17f72   with rel_path: SubDir/abc.txt\r\n  File6: bf41f7ff9743df88a7d0e2f192c50c3f   with rel_path: SubDir/abcHello.txt\r\n  File7: 86fb269d190d2c85f6e0468ceca42a20   with rel_path: SubDir/Hello world!.txt";
            Assert.That(result, Is.EqualTo(EXPECTED_FORMATTED_RESULT));
        }

        [Test]
        public void Test_HashWriter_SHA1_NoFollowing()
        {
            const string HASHING_METHOD = "SHA1";
            IVisitor baseHasher = new HashStreamWriter(HASHING_METHOD);
            HashStreamWriter? hasher = baseHasher as HashStreamWriter;
            Assert.That(hasher, Is.Not.Null);
            Assert.That(hasher.Hasher, Is.EqualTo(HASHING_METHOD));
            IFSBuilder builder = new SimpleFSBuilder(path);
            fs = builder.BuildFileSystem();

            string result = fs.Accept(hasher, path);
            const string EXPECTED_RAW_RESULT = "\r\nda39a3ee5e6b4b0d3255bfef95601890afd80709   with rel_path: empty.txt\r\naf4b47678d863876e068461c95a084601b0c5dd4   with rel_path: normalPlainTree - Shortcut.lnk\r\n\r\na9993e364706816aba3e25717850c26c9cd0d89d   with rel_path: SubDir/abc.txt\r\n6fae8bb42f0ff3f32090ec44646c83b922724af7   with rel_path: SubDir/abcHello.txt\r\n48d8f814da236e27c4cb5794fe2c041cf14555b9   with rel_path: SubDir/empty.txt - Shortcut.lnk\r\nd3486ae9136e7856bc42212385ea797094475802   with rel_path: SubDir/Hello world!.txt\r\n9347cd704681f7214c8f86f68a51996cac8775a6   with rel_path: SubDir/TreeWithLinks - Shortcut.lnk\r\n\r\n";
            Assert.That(result, Is.EqualTo(EXPECTED_RAW_RESULT));
            result = VisitorResultFormatter.FormatResults(result);
            const string EXPECTED_FORMATTED_RESULT = "  File0: da39a3ee5e6b4b0d3255bfef95601890afd80709   with rel_path: empty.txt\r\n  File1: af4b47678d863876e068461c95a084601b0c5dd4   with rel_path: normalPlainTree - Shortcut.lnk\r\n  File2: a9993e364706816aba3e25717850c26c9cd0d89d   with rel_path: SubDir/abc.txt\r\n  File3: 6fae8bb42f0ff3f32090ec44646c83b922724af7   with rel_path: SubDir/abcHello.txt\r\n  File4: 48d8f814da236e27c4cb5794fe2c041cf14555b9   with rel_path: SubDir/empty.txt - Shortcut.lnk\r\n  File5: d3486ae9136e7856bc42212385ea797094475802   with rel_path: SubDir/Hello world!.txt\r\n  File6: 9347cd704681f7214c8f86f68a51996cac8775a6   with rel_path: SubDir/TreeWithLinks - Shortcut.lnk";
            Assert.That(result, Is.EqualTo(EXPECTED_FORMATTED_RESULT));
        }

        [Test]
        public void Test_HashWriter_SHA1_Following()
        {
            const string HASHING_METHOD = "SHA1";
            IVisitor baseHasher = new HashStreamWriter(HASHING_METHOD);
            HashStreamWriter? hasher = baseHasher as HashStreamWriter;
            Assert.That(hasher, Is.Not.Null);
            Assert.That(hasher.Hasher, Is.EqualTo(HASHING_METHOD));

            string result = fs.Accept(hasher, path);
            const string EXPECTED_RAW_RESULT = "\r\n\r\n\r\na9993e364706816aba3e25717850c26c9cd0d89d   with rel_path: ../normalPlainTree/abcOnly/abc.txt\r\n6fae8bb42f0ff3f32090ec44646c83b922724af7   with rel_path: ../normalPlainTree/abcOnly/abcHello.txt\r\n\r\nda39a3ee5e6b4b0d3255bfef95601890afd80709   with rel_path: ../normalPlainTree/empty.txt\r\nd3486ae9136e7856bc42212385ea797094475802   with rel_path: ../normalPlainTree/Hello world!.txt\r\n\r\nda39a3ee5e6b4b0d3255bfef95601890afd80709   with rel_path: empty.txt\r\n\r\na9993e364706816aba3e25717850c26c9cd0d89d   with rel_path: SubDir/abc.txt\r\n6fae8bb42f0ff3f32090ec44646c83b922724af7   with rel_path: SubDir/abcHello.txt\r\nd3486ae9136e7856bc42212385ea797094475802   with rel_path: SubDir/Hello world!.txt\r\n\r\n";
            Assert.That(result, Is.EqualTo(EXPECTED_RAW_RESULT));
            result = VisitorResultFormatter.FormatResults(result);
            const string EXPECTED_FORMATTED_RESULT = "  File0: a9993e364706816aba3e25717850c26c9cd0d89d   with rel_path: ../normalPlainTree/abcOnly/abc.txt\r\n  File1: 6fae8bb42f0ff3f32090ec44646c83b922724af7   with rel_path: ../normalPlainTree/abcOnly/abcHello.txt\r\n  File2: da39a3ee5e6b4b0d3255bfef95601890afd80709   with rel_path: ../normalPlainTree/empty.txt\r\n  File3: d3486ae9136e7856bc42212385ea797094475802   with rel_path: ../normalPlainTree/Hello world!.txt\r\n  File4: da39a3ee5e6b4b0d3255bfef95601890afd80709   with rel_path: empty.txt\r\n  File5: a9993e364706816aba3e25717850c26c9cd0d89d   with rel_path: SubDir/abc.txt\r\n  File6: 6fae8bb42f0ff3f32090ec44646c83b922724af7   with rel_path: SubDir/abcHello.txt\r\n  File7: d3486ae9136e7856bc42212385ea797094475802   with rel_path: SubDir/Hello world!.txt";
            Assert.That(result, Is.EqualTo(EXPECTED_FORMATTED_RESULT));
        }

        [Test]
        public void Test_HashWriter_SHA512_NoFollowing()
        {
            const string HASHING_METHOD = "SHA512";
            IVisitor baseHasher = new HashStreamWriter(HASHING_METHOD);
            HashStreamWriter? hasher = baseHasher as HashStreamWriter;
            Assert.That(hasher, Is.Not.Null);
            Assert.That(hasher.Hasher, Is.EqualTo(HASHING_METHOD));
            IFSBuilder builder = new SimpleFSBuilder(path);
            fs = builder.BuildFileSystem();

            string result = fs.Accept(hasher, path);
            const string EXPECTED_RAW_RESULT = "\r\ncf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e   with rel_path: empty.txt\r\na49bd0fc4a62abaa5ad1f0092976dc0af309379b5bf5ec9e94cc2296871af1351aad24d81ebb537cc6d9a20cba5cea1ee4b1dc706370696a92a44daeae371245   with rel_path: normalPlainTree - Shortcut.lnk\r\n\r\nddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f   with rel_path: SubDir/abc.txt\r\n8ae05990ea27bb2bf36dd9063c723cca7f79481e9b77c09d743558c528d36893802449745e8d6d63941dbbef14f3e6df785d72b1c32d0630b2bd488abcd007b2   with rel_path: SubDir/abcHello.txt\r\n11c7ede1e37d08e33157732873f07ab024aaeb751fdbfa096bc1d8127f35160b00a2fc53fc632985a1d7d0d1ace6e90c1487ee2a7a691d0faa64118b5177db03   with rel_path: SubDir/empty.txt - Shortcut.lnk\r\nf6cde2a0f819314cdde55fc227d8d7dae3d28cc556222a0a8ad66d91ccad4aad6094f517a2182360c9aacf6a3dc323162cb6fd8cdffedb0fe038f55e85ffb5b6   with rel_path: SubDir/Hello world!.txt\r\nb4bed5ea75ac6074a0a6dcde9f12df80922d1b4d2dbfd57514482f68f62c68c3a68b31149220392bcdccd329cb53b2a8cbc7b79be1bc5de5da573a6e808e5faf   with rel_path: SubDir/TreeWithLinks - Shortcut.lnk\r\n\r\n";
            Assert.That(result, Is.EqualTo(EXPECTED_RAW_RESULT));
            result = VisitorResultFormatter.FormatResults(result);
            const string EXPECTED_FORMATTED_RESULT = "  File0: cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e   with rel_path: empty.txt\r\n  File1: a49bd0fc4a62abaa5ad1f0092976dc0af309379b5bf5ec9e94cc2296871af1351aad24d81ebb537cc6d9a20cba5cea1ee4b1dc706370696a92a44daeae371245   with rel_path: normalPlainTree - Shortcut.lnk\r\n  File2: ddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f   with rel_path: SubDir/abc.txt\r\n  File3: 8ae05990ea27bb2bf36dd9063c723cca7f79481e9b77c09d743558c528d36893802449745e8d6d63941dbbef14f3e6df785d72b1c32d0630b2bd488abcd007b2   with rel_path: SubDir/abcHello.txt\r\n  File4: 11c7ede1e37d08e33157732873f07ab024aaeb751fdbfa096bc1d8127f35160b00a2fc53fc632985a1d7d0d1ace6e90c1487ee2a7a691d0faa64118b5177db03   with rel_path: SubDir/empty.txt - Shortcut.lnk\r\n  File5: f6cde2a0f819314cdde55fc227d8d7dae3d28cc556222a0a8ad66d91ccad4aad6094f517a2182360c9aacf6a3dc323162cb6fd8cdffedb0fe038f55e85ffb5b6   with rel_path: SubDir/Hello world!.txt\r\n  File6: b4bed5ea75ac6074a0a6dcde9f12df80922d1b4d2dbfd57514482f68f62c68c3a68b31149220392bcdccd329cb53b2a8cbc7b79be1bc5de5da573a6e808e5faf   with rel_path: SubDir/TreeWithLinks - Shortcut.lnk";
            Assert.That(result, Is.EqualTo(EXPECTED_FORMATTED_RESULT));
        }

        [Test]
        public void Test_HashWriter_SHA512_Following()
        {
            const string HASHING_METHOD = "SHA512";
            IVisitor baseHasher = new HashStreamWriter(HASHING_METHOD);
            HashStreamWriter? hasher = baseHasher as HashStreamWriter;
            Assert.That(hasher, Is.Not.Null);
            Assert.That(hasher.Hasher, Is.EqualTo(HASHING_METHOD));

            string result = fs.Accept(hasher, path);
            const string EXPECTED_RAW_RESULT = "\r\n\r\n\r\nddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f   with rel_path: ../normalPlainTree/abcOnly/abc.txt\r\n8ae05990ea27bb2bf36dd9063c723cca7f79481e9b77c09d743558c528d36893802449745e8d6d63941dbbef14f3e6df785d72b1c32d0630b2bd488abcd007b2   with rel_path: ../normalPlainTree/abcOnly/abcHello.txt\r\n\r\ncf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e   with rel_path: ../normalPlainTree/empty.txt\r\nf6cde2a0f819314cdde55fc227d8d7dae3d28cc556222a0a8ad66d91ccad4aad6094f517a2182360c9aacf6a3dc323162cb6fd8cdffedb0fe038f55e85ffb5b6   with rel_path: ../normalPlainTree/Hello world!.txt\r\n\r\ncf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e   with rel_path: empty.txt\r\n\r\nddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f   with rel_path: SubDir/abc.txt\r\n8ae05990ea27bb2bf36dd9063c723cca7f79481e9b77c09d743558c528d36893802449745e8d6d63941dbbef14f3e6df785d72b1c32d0630b2bd488abcd007b2   with rel_path: SubDir/abcHello.txt\r\nf6cde2a0f819314cdde55fc227d8d7dae3d28cc556222a0a8ad66d91ccad4aad6094f517a2182360c9aacf6a3dc323162cb6fd8cdffedb0fe038f55e85ffb5b6   with rel_path: SubDir/Hello world!.txt\r\n\r\n";
            Assert.That(result, Is.EqualTo(EXPECTED_RAW_RESULT));
            result = VisitorResultFormatter.FormatResults(result);
            const string EXPECTED_FORMATTED_RESULT = "  File0: ddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f   with rel_path: ../normalPlainTree/abcOnly/abc.txt\r\n  File1: 8ae05990ea27bb2bf36dd9063c723cca7f79481e9b77c09d743558c528d36893802449745e8d6d63941dbbef14f3e6df785d72b1c32d0630b2bd488abcd007b2   with rel_path: ../normalPlainTree/abcOnly/abcHello.txt\r\n  File2: cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e   with rel_path: ../normalPlainTree/empty.txt\r\n  File3: f6cde2a0f819314cdde55fc227d8d7dae3d28cc556222a0a8ad66d91ccad4aad6094f517a2182360c9aacf6a3dc323162cb6fd8cdffedb0fe038f55e85ffb5b6   with rel_path: ../normalPlainTree/Hello world!.txt\r\n  File4: cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e   with rel_path: empty.txt\r\n  File5: ddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f   with rel_path: SubDir/abc.txt\r\n  File6: 8ae05990ea27bb2bf36dd9063c723cca7f79481e9b77c09d743558c528d36893802449745e8d6d63941dbbef14f3e6df785d72b1c32d0630b2bd488abcd007b2   with rel_path: SubDir/abcHello.txt\r\n  File7: f6cde2a0f819314cdde55fc227d8d7dae3d28cc556222a0a8ad66d91ccad4aad6094f517a2182360c9aacf6a3dc323162cb6fd8cdffedb0fe038f55e85ffb5b6   with rel_path: SubDir/Hello world!.txt";
            Assert.That(result, Is.EqualTo(EXPECTED_FORMATTED_RESULT));
        }
    }
}
