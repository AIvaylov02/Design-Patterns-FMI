using DP_Task3.FileSystem.Components.Interfaces__ADTs;
using System.Text;

namespace DP_Task3.Visitors.Interfaces__ADTs
{
    public abstract class ADTVisitor : IVisitor
    {
        public string VisitConcreteFile(IMyFile file, string? basePath = null)
        {
            string pathToExecuteFrom = GetPathToExecuteFrom(file, basePath);
            return SpecificFileAction(file) + "   with rel_path: " + pathToExecuteFrom;
        }

        public string VisitDirectory(IMyDirectory directory, string? basePath = null)
        {
            string pathToExecuteFrom = GetPathToExecuteFrom(directory, basePath);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(SpecificDirectoryAction(directory, pathToExecuteFrom)); // this whole line is the specific action
            // 
            foreach (IMyFile child in directory.GetFiles())
            {
                sb.AppendLine(child.Accept(this, basePath));
            }
            return sb.ToString();
        }

        protected abstract string SpecificFileAction(IMyFile file);

        protected abstract string SpecificDirectoryAction(IMyDirectory directory, string pathToExecuteFrom);

        private string GetPathToExecuteFrom(IMyFile file, string? basePath = null)
        {
            string pathToExecuteFrom = file.FilePath;
            if (basePath is not null)
            {
                pathToExecuteFrom = file.GetRelativePath(basePath);
                if (pathToExecuteFrom == "path")
                {
                    pathToExecuteFrom = file.FilePath; // the base path is invalid for this file, continue with absolute paths
                }
            }
            return pathToExecuteFrom;
        }
    }
}
