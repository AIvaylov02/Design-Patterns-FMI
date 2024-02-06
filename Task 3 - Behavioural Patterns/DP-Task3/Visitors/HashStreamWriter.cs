using DP_Task3.FileSystem.Components.Interfaces__ADTs;
using DP_Task3.Hashers;
using DP_Task3.Interfaces;
using DP_Task3.Visitors.Interfaces__ADTs;

namespace DP_Task3.Visitors
{
    public class HashStreamWriter : ADTVisitor
    {
        IChecksumCalculator hasher;
        public readonly static HashSet<string> availableHashers = new HashSet<string>() { "MD5", "SHA1", "SHA512" };
        public HashStreamWriter(string hasherType)
        {
            Hasher = hasherType;
        }
        public string Hasher
        {
            get => GetStringFromChecksumCalculator(hasher);
            set
            {
                hasher = GetChecksumCalculatorFromString(value);
            }
        }

        protected override string SpecificFileAction(IMyFile file)
        {
            return hasher.Calculate(file.FilePath);
        }

        protected override string SpecificDirectoryAction(IMyDirectory directory, string pathToExecuteFrom)
        {
            // hashers don't operate on folders, they only invoke the files, just skip this item
            return string.Empty;
        }

        private static IChecksumCalculator GetChecksumCalculatorFromString(string str)
        {
            switch (str)
            {
                case "MD5":
                    return new MD5Calculator();
                case "SHA1":
                    return new SHA1Calculator();
                case "SHA512":
                    return new SHA512Calculator();
                default:
                    throw new ArgumentException($"Calculator type object given '{str}' is currently not supported!");
            }
        }

        private static string GetStringFromChecksumCalculator(IChecksumCalculator calculator)
        {
            Type objectType = calculator.GetType();
            if (objectType == typeof(MD5Calculator))
                return "MD5";
            else if (objectType == typeof(SHA1Calculator))
                return "SHA1";
            else if (objectType == typeof(SHA512Calculator))
                return "SHA512";
            else
                throw new ArgumentException($"Calculator type object given '{objectType}' is currently not supported!");
        }
    }
}
