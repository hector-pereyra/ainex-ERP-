using System.Diagnostics;

namespace Ainex.CodeBot.Core;

public class BuildRunner
{
    public string Run(string solutionFile)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"build \"{solutionFile}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        var process = Process.Start(psi)!;
        process.WaitForExit();

        return process.StandardOutput.ReadToEnd() +
               process.StandardError.ReadToEnd();
    }
}
