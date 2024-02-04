namespace DP_Task3.Interfaces
{
    public interface IChecksumCalculator
    {
        public string Calculate(Stream reader); // calculates the hash of a binary file Stream

        public string Calculate(string path); // calculates the hash of file's content by specifying the file's name
    }
}
