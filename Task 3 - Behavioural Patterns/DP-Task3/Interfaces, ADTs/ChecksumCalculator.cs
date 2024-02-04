using DP_Task3.Interfaces;

namespace DP_Task3.Interfaces__ADTs
{
    public abstract class ChecksumCalculator : IChecksumCalculator
    {
        public string Calculate(Stream reader) // (Template method) intakes stream and returns a string representation of the hash
        {
            byte[] hash = CalculateHash(reader);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        public string Calculate(string path) // creates a byte fileStream by specifying fileName
        {
            try
            {
                using FileStream stream = File.OpenRead(path);
                return Calculate(stream);
            }
            catch
            {
                Console.WriteLine($"An exception has occured while loading the file {path}. Make sure the name is as expected");
            }
            return "Hash not calculated!";
        }

        protected abstract byte[] CalculateHash(Stream reader); // Generates a byte[] by calculating the hash. It is the only part which is different for each calculator
    }
}
