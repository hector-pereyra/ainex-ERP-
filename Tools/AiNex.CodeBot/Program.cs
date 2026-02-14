using System;
using Ainex.CodeBot.Config;
using Ainex.CodeBot.Core;

var config = new BotConfig();
var scanner = new ProjectScanner();
var generator = new CodeGenerator(config.ApiKey);
var writer = new FileWriter();
var builder = new BuildRunner();

Console.WriteLine("Ainex CodeBot starting...");

// 1. Scan
if (!scanner.InfrastructureExists(config.ProjectRoot))
{
    Console.WriteLine("Infrastructure missing → generating...");

    var structure = scanner.GetStructure(config.ProjectRoot);

    // 2. Generate
    var code = await generator.GenerateInfrastructureAsync(structure);

    // 3. Write
    writer.WriteFiles(config.ProjectRoot, code);
}

// 4. Build
var result = builder.Run(config.SolutionFile);

Console.WriteLine(result);
Console.WriteLine("Done.");
