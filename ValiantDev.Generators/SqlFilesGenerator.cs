using Microsoft.CodeAnalysis;

namespace ValiantDev.Generators;

/// <summary>
/// Finds all files ending with .sql in the project.
/// Generates a public static partial class named SqlFiles with static readonly string fields.
/// Each field has the normalized name of a single .sql file and contains its contents.
/// </summary>
/// <example>var content = SqlFiles.NameOfSqlFile</example>
/// <example>var content = SqlFiles.SubdirectoryName.NameOfSqlFile</example>
[Generator(LanguageNames.CSharp)]
public class SqlFilesGenerator : IIncrementalGenerator
{
    private NestedStaticReadOnlyStringGenerator _generator;

    public SqlFilesGenerator()
    {
        _generator = new NestedStaticReadOnlyStringGenerator("SqlFiles", ".sql");
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        _generator.Execute(context);
    }
}
