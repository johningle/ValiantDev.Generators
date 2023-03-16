using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Generators;

/// <summary>
///     Finds all files ending with .sql in the project.
///     Generates a public static partial class named SqlFiles with const static string fields.
///     Each field has the normalized name of a single .sql file and contains its contents.
/// </summary>
/// <example>var content = SqlFiles.NameOfSqlFile</example>
[Generator(LanguageNames.CSharp)]
public class SqlGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext initContext)
    {
        // find all additional files that end with .sql
        var sqlFiles = initContext.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith(".sql"))
            .Select((additionalText, cancellationToken) => new TextDetail
            {
                Name = Path.GetFileNameWithoutExtension(additionalText.Path),
                Content = SymbolDisplay.FormatLiteral(additionalText.GetText(cancellationToken)!.ToString(), true)
            })
            .Collect();
        
        initContext.RegisterSourceOutput(sqlFiles, (spc, fieldDeclaration) =>
        {
            var generator = new CodeGenerator();
            foreach (var item in fieldDeclaration)
            {
                generator.AddSqlMember(item.Name, item.Content);
            }
            spc.AddSource("Experiments.CanIHazSourceGeneratedSql.SqlFiles.g.cs", generator.GetSourceCode());
        });
    }
}

internal class TextDetail
{
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
