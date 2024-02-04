using DP_Task3.Interfaces__ADTs;
using System.Security.Cryptography;

namespace DP_Task3.Hashers
{
    public class SHA1Calculator : ChecksumCalculator
    {
        protected override byte[] CalculateHash(Stream reader)
        {
            using SHA1 sha1 = SHA1.Create();
            return sha1.ComputeHash(reader);
        }
    }
}
