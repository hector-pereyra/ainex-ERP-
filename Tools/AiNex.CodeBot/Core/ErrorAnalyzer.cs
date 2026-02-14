using System.Text.RegularExpressions;

namespace Ainex.CodeBot.Core;

public class ErrorAnalyzer
{
    public bool HasErrors(string buildOutput)
    {
        if (string.IsNullOrWhiteSpace(buildOutput))
            return false;

        return buildOutput.Contains("error CS") ||
               buildOutput.Contains("Build FAILED") ||
               buildOutput.Contains(": error ");
    }

    public string ExtractErrors(string buildOutput)
    {
        if (string.IsNullOrWhiteSpace(buildOutput))
            return string.Empty;

        var lines = buildOutput.Split('\n');

        var errors = lines
            .Where(l => l.Contains(": error"))
            .Take(50);

        return string.Join("\n", errors);
    }
}
