using DP_Task3.FileSystem.Builders;
using DP_Task3.FileSystem.Components.Interfaces__ADTs;
using DP_Task3.Hashers;
using DP_Task3.Visitors;
using DP_Task3.Visitors.Interfaces__ADTs;

namespace DP_Task3
{
    public class Program
    {
        static void Main(string[] args)
        {
            string path = EnterPath("for the scanning process", Console.In, Console.Out); // ../../../../DP-Task3-UnitTests/Testing-file-system/TreeWithLinks
            bool followLinks = EnterLinksFollowing(Console.In, Console.Out);
            IFSBuilder builder = GenerateBuilderOfFileSystem(path, followLinks);
            IMyFile filesystem = builder.BuildFileSystem();

            string outputFilePath = EnterPath("Specify the path with its name where you wish the resulting file of the scan to be saved", Console.In, Console.Out);
            // ../../../../DP-Task3-UnitTests/Testing-file-system/allFiles/test.txt

            IVisitor hasher = GenerateAppropriateHashingVisitor(Console.In, Console.Out);
            IVisitor sizeCalculator = new ReportWriter();
            WriteInfoToFile(filesystem, path, outputFilePath, hasher, sizeCalculator);
        }

        static string EnterPath(string endmessage, TextReader inputStream, TextWriter outputStream)
        {
            outputStream.WriteLine($"Specify the path {endmessage}. (Absolute/relative)");
            string? path = inputStream.ReadLine();
            while (path is null)
            {
                outputStream.WriteLine("Please enter a valid path!");
            }
            // we have a path, convert it to absolute
            return HasherHelperFunctions.ConvertPathToAbsolute(path);
        }

        static bool EnterLinksFollowing(TextReader inputStream, TextWriter outputStream)
        {
            outputStream.WriteLine("Do you want to include linked directories/files or do you want to just see the shortcuts");
            HashSet<string> allowedInclusions = new HashSet<string>() {
                "I", "i", "Include", "include", "INCLUDE"
            };
            outputStream.WriteLine($"Type one of the [ {String.Join(", ", allowedInclusions)} ] sequences to follow it");
            string? input = inputStream.ReadLine();
            if (input is null)
            {
                return false;
            }
            else
            {
                return allowedInclusions.Contains(input);
            }
        }

        static IFSBuilder GenerateBuilderOfFileSystem(string absolutePath, bool followLinks)
        {
            if (followLinks)
            {
                return new AdvancedFSBuilder(absolutePath);
            }
            else
            {
                return new SimpleFSBuilder(absolutePath);
            }
        }

        static IVisitor GenerateAppropriateHashingVisitor(TextReader inputStream, TextWriter outputStream)
        {
            while (true) // will run until the user gives the builder an appropriate supported method for file hashing
            {
                try
                {
                    outputStream.WriteLine("Enter the designated checksum algorithm you wish to use. Available algorithms are:");
                    outputStream.WriteLine(String.Join(", ", HashStreamWriter.availableHashers));
                    string? hashingMethod = inputStream.ReadLine();
                    while (hashingMethod is null)
                    {
                        outputStream.WriteLine("Enter the designated checksum algorithm you wish to use. Available algorithms are:");
                        outputStream.WriteLine(String.Join(", ", HashStreamWriter.availableHashers));
                        hashingMethod = inputStream.ReadLine();
                    }
                    return new HashStreamWriter(hashingMethod);
                }
                catch (ArgumentException)
                {
                    outputStream.WriteLine("Inputted hashing method is unsupported, please choose another!");
                    outputStream.WriteLine();
                }
            }
            
        }

        static void WriteInfoToFile(IMyFile filesystem, string basePath, string outputFilePath, IVisitor hashReporter, IVisitor calculator)
        {
            using StreamWriter writer = new StreamWriter(outputFilePath);
            string valuableInformation = filesystem.Accept(calculator, basePath);
            valuableInformation = VisitorResultFormatter.FormatResults(valuableInformation);
            writer.WriteLine(valuableInformation);
            writer.WriteLine();
            valuableInformation = filesystem.Accept(hashReporter, basePath);
            valuableInformation = VisitorResultFormatter.FormatResults(valuableInformation);
            writer.WriteLine(valuableInformation);
            writer.WriteLine();
            writer.Close();
        }
    }
}
