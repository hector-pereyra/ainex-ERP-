using System;

namespace Ainex.CodeBot.Core;

public class AutoFixLoop
{
    private readonly BuildRunner _builder;
    private readonly ErrorAnalyzer _analyzer;
    private readonly CodeGenerator _generator;
    private readonly FileWriter _writer;
    private readonly ProjectScanner _scanner;
    private readonly string _root;
    private readonly string _solution;

    public AutoFixLoop(
        BuildRunner builder,
        ErrorAnalyzer analyzer,
        CodeGenerator generator,
        FileWriter writer,
        ProjectScanner scanner,
        string root,
        string solution)
    {
        _builder = builder;
        _analyzer = analyzer;
        _generator = generator;
        _writer = writer;
        _scanner = scanner;
        _root = root;
        _solution = solution;
    }

    public async Task RunAsync(int maxAttempts = 5)
    {
        for (int i = 1; i <= maxAttempts; i++)
        {
            Console.WriteLine($"Build attempt {i}...");

            var result = _builder.Run(_solution);

            if (!_analyzer.HasErrors(result))
            {
                Console.WriteLine("Build OK.");
                return;
            }

            Console.WriteLine("Errors detected → fixing...");

            var errors = _analyzer.ExtractErrors(result);
            var structure = _scanner.GetStructure(_root);

            var fixedCode = await _generator.FixBuildErrorsAsync(errors, structure);
            _writer.WriteFiles(_root, fixedCode);
        }

        Console.WriteLine("AutoFix stopped. Manual review required.");
    }
}
