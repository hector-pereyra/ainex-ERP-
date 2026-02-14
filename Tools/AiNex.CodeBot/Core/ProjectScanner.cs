using System.IO;
using System.Linq;

namespace Ainex.CodeBot.Core;

public class ProjectScanner
{
    public bool InfrastructureExists(string root)
    {
        var path = Path.Combine(root, "backend", "Ainex.Infrastructure");
        return Directory.Exists(path);
    }

    public string GetStructure(string root)
    {
        if (!Directory.Exists(root))
            return string.Empty;

        return string.Join("\n",
            Directory.GetDirectories(root, "*", SearchOption.AllDirectories)
                     .Take(200));
    }
}
