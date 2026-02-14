using System.IO;
using System.Linq;

namespace Ainex.CodeBot.Core;

public class FileWriter
{
    public void WriteFiles(string root, string generated)
    {
        var parts = generated.Split("===FILE:");
        foreach (var part in parts.Skip(1))
        {
            var endName = part.IndexOf("===");
            if (endName < 0) continue;

            var fileName = part.Substring(0, endName).Trim();
            var content = part[(endName + 3)..];

            var fullPath = Path.Combine(root, "backend", "Ainex.Infrastructure", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            File.WriteAllText(fullPath, content);
        }
    }
}
