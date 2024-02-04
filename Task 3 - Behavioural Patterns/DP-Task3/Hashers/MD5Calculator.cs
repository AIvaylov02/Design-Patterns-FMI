using DP_Task3.Interfaces__ADTs;
using System.Security.Cryptography;

namespace DP_Task3.Hashers
{
    public class MD5Calculator : ChecksumCalculator
    {
        protected override byte[] CalculateHash(Stream reader)
        {
            using MD5 md5 = MD5.Create();
            return md5.ComputeHash(reader);
        }
    }
}
