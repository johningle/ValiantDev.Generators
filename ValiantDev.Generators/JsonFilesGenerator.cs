using Microsoft.CodeAnalysis;

namespace ValiantDev.Generators;

/// <summary>
/// Finds all files ending with .json in the project.
/// Generates a public static partial class named SqlFiles with static readonly string fields.
/// Each field has the normalized name of a single .json file and contains its contents.
/// </summary>
/// <example>var jsonString = JsonFiles.NameOfJsonFile</example>
/// <example>var jsonString = JsonFiles.SubdirectoryName.NameOfJsonFile</example>
[Generator(LanguageNames.CSharp)]
public class JsonFilesGenerator : IIncrementalGenerator
{
    private NestedStaticReadOnlyStringGenerator _generator;

    public JsonFilesGenerator()
    {
        _generator = new NestedStaticReadOnlyStringGenerator("JsonFiles", ".json");
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        _generator.Execute(context);
    }
}
