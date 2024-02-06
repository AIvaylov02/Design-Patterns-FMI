using DP_Task3.FileSystem.Components.Interfaces__ADTs;
using DP_Task3.Visitors.Interfaces__ADTs;
using System.Text;

namespace DP_Task3.Visitors
{
    public class ReportWriter : ADTVisitor
    {
        protected override string SpecificFileAction(IMyFile file)
        {
            ulong size = file.CalculateSize(); // 5 stages
            return FormatSize(size);
        }

        protected override string SpecificDirectoryAction(IMyDirectory directory, string pathToExecuteFrom)
        {
            ulong size = directory.CalculateSize();
            return $"{FormatSize(size)}   with rel_path: {pathToExecuteFrom}";
        }

        private string FormatSize(ulong size)
        {
            StringBuilder sb = new StringBuilder(" ");
            ulong temp = size;
            if (temp == 0) // corner case check to get us on track if a file is empty
            {
                temp = 1;
            }
            while (temp < 1000) // append a leading white space for every missing digit
            {
                sb.Append(" "); // 3 will be formatted to "0003"
                temp *= 10;
            }
            sb.Append(size);
            return sb.ToString();
        }
    }
}
