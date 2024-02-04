namespace DP_Task3.FileSystem.Components.Interfaces__ADTs
{
    public interface IMyFile : IEquatable<IMyFile>
    {
        // hypothetically most modern desktop systems are of size 1 TB - 2 TB, so 1 TB is 
        // 1,099,511,627,776
        // 18,446,744,073,709,551,615 is the max ulong value, that means we can have a file as large as 16'777'215 TB, which is the maximum NTFS size in Windows
        // Theoretically we should use Big Integer for the task but I will stick to ulong as I think Big Int is overkill, as few of the systems I know barely exceed the 2TB limit.
        public ulong CalculateSize();

        public string FilePath { get; } 
    }
}
