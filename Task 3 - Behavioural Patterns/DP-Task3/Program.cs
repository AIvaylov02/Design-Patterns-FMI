using DP_Task3.Hashers;
using DP_Task3.Interfaces;
using System.Text;

namespace DP_Task3
{
    public class Program
    {
        static void Main(string[] args)
        {
            IChecksumCalculator checksumCalculator = new MD5Calculator();
            using Stream stream = HasherHelperFunctions.GenerateStream("abc");
            string result = checksumCalculator.Calculate(stream);
            string EXPECTED_RESULT = "900150983cd24fb0d6963f7d28e17f72";
            Console.WriteLine(result);
            Console.WriteLine(EXPECTED_RESULT == result);

            checksumCalculator = new SHA1Calculator();
            string path = HasherHelperFunctions.GetFullPathToFile("test.txt");
            result = checksumCalculator.Calculate(path);

            EXPECTED_RESULT = "a9993e364706816aba3e25717850c26c9cd0d89d";
            Console.WriteLine(result);
            Console.WriteLine(EXPECTED_RESULT == result);

            using Stream stream3 = HasherHelperFunctions.GenerateStream("abc");
            checksumCalculator = new SHA512Calculator();
            result = checksumCalculator.Calculate(stream3);
            EXPECTED_RESULT = "ddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f";
            Console.WriteLine(result);
            Console.WriteLine(EXPECTED_RESULT == result);
        }
    }
}
