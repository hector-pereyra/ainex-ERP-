using System.IO;

namespace Ainex.CodeBot.Config;

public class BotConfig
{
    public string ProjectRoot { get; set; } = @"./";
    public string SolutionFile => Path.Combine(ProjectRoot, "ainex.sln");

    // IMPORTANTE: luego lo moveremos a variable de entorno
    public string ApiKey { get; set; } = "OPENAI_API_KEY";
}
