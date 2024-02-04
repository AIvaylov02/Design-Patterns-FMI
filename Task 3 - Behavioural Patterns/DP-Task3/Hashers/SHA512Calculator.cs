using DP_Task3.Interfaces__ADTs;
using System.Security.Cryptography;

namespace DP_Task3.Hashers
{
    public class SHA512Calculator : ChecksumCalculator
    {
        protected override byte[] CalculateHash(Stream reader)
        {
            using SHA512 sha512 = SHA512.Create();
            return sha512.ComputeHash(reader);
        }
    }
}
