using DP_Task3.FileSystem.Components.Interfaces__ADTs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP_Task3.Visitors.Interfaces__ADTs
{
    public interface IVisitor
    {
        string VisitConcreteFile(IMyFile file, string? basePath = null); // will get formatted to the basePath of choosing (if null absolute paths will be used)

        string VisitDirectory(IMyDirectory directory, string? basePath = null);
    }
}
