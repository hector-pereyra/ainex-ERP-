using System.Diagnostics;

namespace Ainex.CodeBot.Core;

public class MigrationRunner
{
    public string AddMigration(string projectPath, string name = "InitialCreate")
    {
        return Run($"ef migrations add {name} --project \"{projectPath}\"");
    }

    public string UpdateDatabase(string projectPath)
    {
        return Run($"ef database update --project \"{projectPath}\"");
    }

    private string Run(string args)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = args,
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
