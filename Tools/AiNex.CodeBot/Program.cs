using System;
using Ainex.CodeBot.Config;
using Ainex.CodeBot.Core;

var config = new BotConfig();

var scanner = new ProjectScanner();
var generator = new CodeGenerator(config.ApiKey);
var writer = new FileWriter();
var builder = new BuildRunner();
var analyzer = new ErrorAnalyzer();
var migrations = new MigrationRunner();

Console.WriteLine("Ainex CodeBot starting...");

// 1. Generar Infrastructure si no existe
/*if (!scanner.InfrastructureExists(config.ProjectRoot))
{
    Console.WriteLine("Infrastructure missing → generating...");

    var structure = scanner.GetStructure(config.ProjectRoot);
    var code = await generator.GenerateInfrastructureAsync(structure);
    writer.WriteFiles(config.ProjectRoot, code);
}*/
// 1. Generar Infrastructure si no existe
if (!scanner.InfrastructureExists(config.ProjectRoot))
{
    Console.WriteLine("Infrastructure missing → generating...");

    var structure = scanner.GetStructure(config.ProjectRoot);
    var code = await generator.GenerateInfrastructureAsync(structure);
    writer.WriteFiles(config.ProjectRoot, code);
}

// 2. Generar DbContext + DB config
Console.WriteLine("Generating DbContext...");

{
    var structure = scanner.GetStructure(config.ProjectRoot);
    var dbCode = await generator.GenerateDbContextAsync(structure);
    writer.WriteFiles(config.ProjectRoot, dbCode);
}
// 3. Generar Repositories + UnitOfWork
Console.WriteLine("Generating Repositories...");

{
    var structure = scanner.GetStructure(config.ProjectRoot);
    var repoCode = await generator.GenerateRepositoriesAsync(structure);
    writer.WriteFiles(config.ProjectRoot, repoCode);
}
// 4. Generar módulo Auth + JWT
Console.WriteLine("Generating Auth module...");

{
    var structure = scanner.GetStructure(config.ProjectRoot);
    var authCode = await generator.GenerateAuthModuleAsync(structure);
    writer.WriteFiles(config.ProjectRoot, authCode);
}

// N. AutoFix Loop (semi-autónomo)
var loop = new AutoFixLoop(
    builder,
    analyzer,
    generator,
    writer,
    scanner,
    config.ProjectRoot,
    config.SolutionFile);

await loop.RunAsync();

// 5. Ejecutar migraciones EF
Console.WriteLine("Running EF migrations...");

var infraProject = Path.Combine(config.ProjectRoot, "backend", "Ainex.Infrastructure", "Ainex.Infrastructure.csproj");

Console.WriteLine(migrations.AddMigration(infraProject));
Console.WriteLine(migrations.UpdateDatabase(infraProject));

// 6. Generar módulo Clientes
Console.WriteLine("Generating Customers module...");

{
    var structure = scanner.GetStructure(config.ProjectRoot);
    var customersCode = await generator.GenerateCustomersModuleAsync(structure);
    writer.WriteFiles(config.ProjectRoot, customersCode);
}

// 7. Generar módulo Productos + Inventario
Console.WriteLine("Generating Products & Inventory module...");

{
    var structure = scanner.GetStructure(config.ProjectRoot);
    var prodCode = await generator.GenerateProductsInventoryModuleAsync(structure);
    writer.WriteFiles(config.ProjectRoot, prodCode);
}

// 8. Generar módulo Ventas / Facturación
Console.WriteLine("Generating Sales module...");

{
    var structure = scanner.GetStructure(config.ProjectRoot);
    var salesCode = await generator.GenerateSalesModuleAsync(structure);
    writer.WriteFiles(config.ProjectRoot, salesCode);
}

// 9. Hardening MVP
Console.WriteLine("Applying MVP Hardening...");

{
    var structure = scanner.GetStructure(config.ProjectRoot);
    var hardeningCode = await generator.GenerateHardeningAsync(structure);
    writer.WriteFiles(config.ProjectRoot, hardeningCode);
}
Console.WriteLine("CodeBot finished.");
