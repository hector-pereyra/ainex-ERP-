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
    public async Task<string> GenerateAuthModuleAsync(string projectStructure)
{
    var prompt = $@"
You are generating production-ready .NET code.

Task:
Create Users + Authentication module with JWT using Clean Architecture.

Requirements:

- Entity: User (Id, Email, PasswordHash, IsActive, CreatedAt)
- Basic Role support
- Password hashing (BCrypt or similar)
- JWT Token generator
- Login endpoint: /auth/login
- Register endpoint: /auth/register
- ASP.NET Core Authentication with JWT Bearer
- Middleware configured
- Seed admin user
- DI configuration included

Return ONLY code files in this format:

===FILE: filename.cs===
<code>

Project structure:
{projectStructure}
";

    var response = await _chat.CompleteChatAsync(prompt);
    return response.Content[0].Text;
    }

    public async Task<string> GenerateCustomersModuleAsync(string projectStructure)
{
    var prompt = $@"
You are generating production-ready .NET ERP module.

Task:
Create Customers module using Clean Architecture.

Requirements:

Domain:
- Entity: Customer (Id, Name, Email, Phone, Address, CreatedAt)

Infrastructure:
- EF Core configuration
- Use existing Repository<T> and UnitOfWork

API:
- CustomersController
- Endpoints:
  GET /customers
  GET /customers/{{id}}
  POST /customers
  PUT /customers/{{id}}
  DELETE /customers/{{id}}

Rules:
- Use async/await
- Protect with [Authorize]
- Use DTOs if needed
- Register services in DI

Return ONLY code files in this format:

===FILE: filename.cs===
<code>

Project structure:
{projectStructure}
";

    var response = await _chat.CompleteChatAsync(prompt);
    return response.Content[0].Text;
    }

    public async Task<string> GenerateProductsInventoryModuleAsync(string projectStructure)
{
    var prompt = $@"
You are generating production-ready .NET ERP module.

Task:
Create Products + Inventory module using Clean Architecture.

Domain:
- Entity Product (Id, Name, SKU, Price, IsActive, CreatedAt)
- Entity Inventory (Id, ProductId, Quantity)
- Entity InventoryTransaction (Id, ProductId, Quantity, Type, CreatedAt)

Infrastructure:
- EF Core configurations
- Use Repository<T> and UnitOfWork

API:
- ProductsController
  GET /products
  GET /products/{{id}}
  POST /products
  PUT /products/{{id}}
  DELETE /products/{{id}}

- Inventory endpoints:
  GET /inventory
  POST /inventory/adjust

Rules:
- Async/await
- Protect with [Authorize]
- DTOs allowed
- Update stock on adjustments
- Register services in DI

Return ONLY code files in this format:

===FILE: filename.cs===
<code>

Project structure:
{projectStructure}
";

    var response = await _chat.CompleteChatAsync(prompt);
    return response.Content[0].Text;
    }

    public async Task<string> GenerateSalesModuleAsync(string projectStructure)
{
    var prompt = $@"
You are generating production-ready .NET ERP module.

Task:
Create Sales / Invoicing module using Clean Architecture.

Domain:
- Entity Sale (Id, CustomerId, Total, CreatedAt)
- Entity SaleItem (Id, SaleId, ProductId, Quantity, UnitPrice, Total)

Rules:
- On sale creation, decrease inventory
- Use Repository<T> and UnitOfWork
- Async/await
- Protected with [Authorize]

API:
- SalesController
  POST /sales (create sale with items)
  GET /sales
  GET /sales/{{id}}

Requirements:
- Transactional integrity
- DTOs allowed
- Integrate with Customer + Product + Inventory
- Register services in DI

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
