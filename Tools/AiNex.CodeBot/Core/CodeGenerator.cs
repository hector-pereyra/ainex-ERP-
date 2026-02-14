using System.Threading.Tasks;
using OpenAI.Chat;

namespace Ainex.CodeBot.Core;

public class CodeGenerator
{
    private readonly ChatClient _chat;

    public CodeGenerator(string apiKey)
    {
        _chat = new ChatClient(model: "gpt-5.2", apiKey: apiKey);
    }

    public async Task<string> GenerateInfrastructureAsync(string projectStructure)
    {
        var prompt = $@"
Generate production-ready .NET Infrastructure layer using:

- Clean Architecture
- EF Core
- PostgreSQL
- DbContext
- Base Repository
- Dependency Injection

Project structure:
{projectStructure}

Return ONLY code files separated exactly like:

===FILE: filename.cs===
<code>
";

        var response = await _chat.CompleteChatAsync(prompt);
        return response.Content[0].Text;
    }

public async Task<string> FixBuildErrorsAsync(string errors, string projectStructure)
{
    var prompt = $@"
You are fixing a .NET build.

Goal:
Fix compilation errors in the Infrastructure layer.

Rules:
- Return ONLY corrected code files
- Same format:

===FILE: filename.cs===
<code>

Build errors:
{errors}

Project structure:
{projectStructure}
";

    var response = await _chat.CompleteChatAsync(prompt);
    return response.Content[0].Text;
    }

    public async Task<string> GenerateDbContextAsync(string projectStructure)
{
    var prompt = $@"
You are generating production-ready .NET code.

Task:
Create DbContext and database configuration for Infrastructure layer using:

- EF Core
- PostgreSQL
- Clean Architecture
- DbContext named AinexDbContext
- OnModelCreating configured
- Basic entity placeholders allowed
- Dependency Injection extension

Return ONLY code files in this format:

===FILE: filename.cs===
<code>

Project structure:
{projectStructure}
";

    var response = await _chat.CompleteChatAsync(prompt);
    return response.Content[0].Text;
    }
    public async Task<string> GenerateRepositoriesAsync(string projectStructure)
{
    var prompt = $@"
You are generating production-ready .NET code.

Task:
Create repository pattern for Infrastructure layer using:

- Clean Architecture
- EF Core
- Generic Repository
- UnitOfWork
- Async methods
- Dependency Injection

Requirements:
- Interface: IRepository<T>
- Implementation: Repository<T>
- IUnitOfWork
- UnitOfWork using AinexDbContext
- AddInfrastructure extension must register repositories

Return ONLY code files in this format:

===FILE: filename.cs===
<code>

Project structure:
{projectStructure}
";

    var response = await _chat.CompleteChatAsync(prompt);
    return response.Content[0].Text;
    }
}
